using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class TestCategoryService(UnitOfWork unitOfWork) : ITestCategoryService
{
    public async Task<IList<TestCategoryDto>> FindAllAsync()
    {
        var testCategoryEntities =
            await unitOfWork.TestCategoryRepository.FindAllAsync();

        return testCategoryEntities.Adapt<List<TestCategoryDto>>();
    }
}