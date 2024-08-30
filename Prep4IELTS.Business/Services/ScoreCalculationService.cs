using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class ScoreCalculationService(UnitOfWork unitOfWork) : IScoreCalculationService
{
    public async Task<ScoreCalculationDto> GetByTotalRightAnswerAndTestType(int totalRightAnswer, string testType)
    {
        var scoreCalculationEntity =
            await unitOfWork.ScoreCalculationRepository.GetByTotalRightAnswerAndTestType(totalRightAnswer, testType);
        return scoreCalculationEntity.Adapt<ScoreCalculationDto>();
    }
}