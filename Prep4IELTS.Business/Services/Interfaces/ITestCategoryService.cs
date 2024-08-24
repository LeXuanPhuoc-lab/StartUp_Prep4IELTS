using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITestCategoryService
{
    Task<IList<TestCategoryDto>> FindAllAsync();
}