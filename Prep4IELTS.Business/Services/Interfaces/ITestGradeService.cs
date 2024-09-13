using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITestGradeService
{
    Task<IList<TestGradeDto>> FindAllTestGradeByHistoryId(int testHistoryId);
    Task<bool> UpdateTestGradesAsync(List<TestGradeDto> testGrades, int testHistoryId, int totalCompletionTime, DateTime takenDate);
}