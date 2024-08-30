using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class QuestionService(UnitOfWork unitOfWork) : IQuestionService
{
    public async Task<QuestionDto> FindQuestionByIdAndWithQuestions(int questionId)
    {
        var questionEntity = await unitOfWork.QuestionRepository.FindQuestionByIdAndWithQuestions(questionId);
        return questionEntity.Adapt<QuestionDto>();
    }
}