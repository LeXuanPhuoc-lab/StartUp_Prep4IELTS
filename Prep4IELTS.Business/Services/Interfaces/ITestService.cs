using System.Linq.Expressions;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITestService
{
    public Task<bool> InsertAsync(TestDto test);
    public Task<bool> RemoveAsync(Guid id);
    public Task UpdateAsync(TestDto test);
    public Task<TestDto> FindAsync(Guid id);
    public Task<IList<TestDto>> FindAllAsync();

    public Task<IList<TestDto>> FindWithConditionAsync(
        Expression<Func<Test, bool>>? filter = null,
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy = null,
        string? includeProperties = "");
}