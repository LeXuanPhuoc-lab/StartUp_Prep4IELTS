using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITestService
{
    // Basic
    Task<bool> InsertAsync(TestDto test);
    Task<bool> RemoveAsync(Guid id);
    Task UpdateAsync(TestDto test);
    Task<TestDto> FindAsync(Guid id);
    Task<IList<TestDto>> FindAllAsync();
    Task<TestDto> FindOneWithConditionAsync(
        Expression<Func<Test, bool>> filter,
        string? includeProperties = "");
    Task<IList<TestDto>> FindAllWithConditionAsync(
        Expression<Func<Test, bool>>? filter = null,
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy = null,
        string? includeProperties = "");
    Task<IList<TestDto>> FindAllWithConditionAndThenIncludeAsync(
        Expression<Func<Test, bool>>? filter = null,
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy = null,
        List<Func<IQueryable<Test>, IIncludableQueryable<Test, object>>>? includes = null);
    
    // Additional
    Task<IList<TestDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Test, bool>>? filter,
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize,
        // Include test histories for user (if any)
        string? userId);

    Task<TestDto> FindByIdAsync(int id, Guid? userId);
    Task<IList<TestDto>> FindByIdForPracticeAsync(int id, int[] sectionIds);
    Task<IList<TestDto>> FindByIdForTestSimulationAsync(int id);
    Task<int> CountTotalAsync();
}