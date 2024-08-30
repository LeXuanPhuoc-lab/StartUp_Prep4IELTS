using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IQuestionService
{
    Task<QuestionDto> FindQuestionByIdAndWithQuestions(int questionId);
}