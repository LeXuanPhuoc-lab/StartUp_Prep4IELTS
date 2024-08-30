using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IScoreCalculationService
{
    Task<ScoreCalculationDto> GetByTotalRightAnswerAndTestType(int totalRightAnswer, string testType);
}