using Mapster;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Business.Utils;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;
using System.Linq;
using System.Linq.Expressions;

namespace Prep4IELTS.Business.Services;

public class FlashcardExamHistoryService(
    IFlashcardDetailService flashcardDetailService,
    IVocabularyUnitScheduleService vocabularyUnitScheduleService,
    UnitOfWork unitOfWork) : IFlashcardExamHistoryService
{
    public async Task<bool> InsertAsync(
        FlashcardExamHistoryDto flashcardExamHistoryDto, int userFlashcardId,
        bool isTermPattern, bool? isSaveWrongToVocabSchedule)
    {
        // Map to FlashcardExamHistory entity
        var flashcardExamHistoryEntity = flashcardExamHistoryDto.Adapt<FlashcardExamHistory>();
        
        // Grade for result
        foreach (var eg in flashcardExamHistoryEntity.FlashcardExamGrades)
        {
            // Get flashcard detail
            var flashcardDetailDto = await flashcardDetailService.FindByIdAsync(eg.FlashcardDetailId);
            if (flashcardDetailDto == null) return false; // TODO: use IServiceResult to handle this instead 
            
            // Progress grade status for flashcard exam grade
            if (string.IsNullOrEmpty(eg.Answer)) // Check answer is empty
            {
                // Assign Skip grade status
                eg.FlashcardGradeStatus = FlashcardGradeStatus.Skip.GetDescription();
            }
            else // Check for answer correctness
            {
                // Get correct answer
                var correctAnswer = isTermPattern
                    ? flashcardDetailDto.Definition
                    : flashcardDetailDto.WordText;
                // Check whether is correct 
                var isCorrect = eg.QuestionType == FlashcardExamTypeConstants.Written
                    ? StringUtils.CompareAnswers(correctAnswer, eg.Answer)
                    : correctAnswer == eg.Answer;
                // Update grade status 
                eg.FlashcardGradeStatus = isCorrect 
                    ? FlashcardGradeStatus.Correct.GetDescription() 
                    : FlashcardGradeStatus.Wrong.GetDescription();
                // Increase total report figures
                if (isCorrect) flashcardExamHistoryEntity.TotalRightAnswer++;
                else flashcardExamHistoryEntity.TotalWrongAnswer++;
                
                // Check whether save wrong answer to vocab schedule
                if (!isCorrect // Is wrong answer
                    && isSaveWrongToVocabSchedule.HasValue && isSaveWrongToVocabSchedule.Value) // Save wrong required
                {
                    var vocabUnitScheduleDto = new VocabularyUnitScheduleDto(
                        VocabularyUnitScheduleId: 0,
                        FlashcardDetailId: flashcardDetailDto.FlashcardDetailId,
                        UserFlashcardId: userFlashcardId,
                        CreateDate: flashcardExamHistoryDto.TakenDate,
                        Weekday: null!,
                        Comment: null!,
                        FlashcardDetail: null!,
                        UserFlashcard: null!);

                    await vocabularyUnitScheduleService.InsertAsync(vocabUnitScheduleDto);
                }
            }
        }
        
        // Update history report elements
        flashcardExamHistoryEntity.TotalRightAnswer = 
            flashcardExamHistoryEntity.FlashcardExamGrades.Count(fg => 
                fg.FlashcardGradeStatus == FlashcardGradeStatus.Correct.GetDescription());
        flashcardExamHistoryEntity.TotalWrongAnswer = 
            flashcardExamHistoryEntity.FlashcardExamGrades.Count(fg => 
                fg.FlashcardGradeStatus == FlashcardGradeStatus.Wrong.GetDescription());
        
        // Calculate accuracy rate
        flashcardExamHistoryEntity.AccuracyRate =
            flashcardExamHistoryEntity.TotalQuestion > 0
                ? flashcardExamHistoryEntity.TotalRightAnswer / flashcardExamHistoryEntity.TotalQuestion
                : 0;
        
        // Progress insert flashcard exam history
        await unitOfWork.FlashcardExamHistoryRepository.InsertAsync(flashcardExamHistoryEntity);
        return await unitOfWork.FlashcardExamHistoryRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<FlashcardExamHistoryDto?> FindByUserFlashcardIdAtTakenDateAsync(int userFlashcardId, DateTime takenDateTime)
    {
        var flashcardExamHisEntity =
            await unitOfWork.FlashcardExamHistoryRepository.FindByUserFlashcardIdAtTakenDateAsync(userFlashcardId,
                takenDateTime);
        return flashcardExamHisEntity.Adapt<FlashcardExamHistoryDto>();
    }

    public async Task<List<FlashcardExamHistoryDto>> FindAllByUserAsync(
        Expression<Func<FlashcardExamHistory, bool>>? filter, 
        Func<IQueryable<FlashcardExamHistory>, IOrderedQueryable<FlashcardExamHistory>>? orderBy,
        Guid userId)
    {
        var flashcardExamHisEntities =
            await unitOfWork.FlashcardExamHistoryRepository.FindAllByUserAsync(filter, orderBy, userId);
        return flashcardExamHisEntities.Adapt<List<FlashcardExamHistoryDto>>();
    }
}