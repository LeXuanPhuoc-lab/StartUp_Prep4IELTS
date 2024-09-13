using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class TestGradeService(UnitOfWork unitOfWork) : ITestGradeService
{
    public async Task<IList<TestGradeDto>> FindAllTestGradeByHistoryId(int testHistoryId)
    {
        var testGradeEntities = 
            await unitOfWork.TestGradeRepository.FindAllTestGradeByHistoryId(testHistoryId);
        return testGradeEntities.Adapt<List<TestGradeDto>>();
    }

    public async Task<bool> UpdateTestGradesAsync(List<TestGradeDto> testGrades, int testHistoryId, int totalCompletionTime, DateTime takenDate)
    {
        return await unitOfWork.TestGradeRepository.UpdateTestGradesAsync(
            testGrades.Adapt<List<TestGrade>>(), testHistoryId, totalCompletionTime, takenDate);
    }
}