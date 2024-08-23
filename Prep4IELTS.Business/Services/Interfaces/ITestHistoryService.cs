using System.Linq.Expressions;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITestHistoryService
{
    Task<bool> InsertAsync(TestHistoryDto test);
    Task<bool> RemoveAsync(int id);
    Task UpdateAsync(TestHistoryDto test);
    Task<TestHistoryDto> FindAsync(int id);
    Task<IList<TestHistoryDto>> FindAllAsync();
    Task<TestHistoryDto> FindOneWithConditionAsync(
        Expression<Func<TestHistory, bool>> filter,
        string? includeProperties = "");
    Task<IList<TestHistoryDto>> FindAllWithConditionAsync(
        Expression<Func<TestHistory, bool>>? filter = null,
        Func<IQueryable<TestHistory>, IOrderedQueryable<TestHistory>>? orderBy = null,
        string? includeProperties = "");
}