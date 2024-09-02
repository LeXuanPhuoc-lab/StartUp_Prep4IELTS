using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITestSectionService
{
    // Basic
    Task<bool> InsertAsync(TestSectionDto testSection);
    Task<bool> RemoveAsync(int id);
    Task UpdateAsync(TestSectionDto testSection);
    Task<TestSectionDto> FindAsync(int id);
    Task<IList<TestSectionDto>> FindAllAsync();
    Task<TestSectionDto> FindOneWithConditionAsync(
        Expression<Func<TestSection, bool>>? filter,
        Func<IQueryable<TestSection>, IOrderedQueryable<TestSection>>? orderBy = null, 
        string? includeProperties = "");
    Task<IList<TestSectionDto>> FindAllWithConditionAsync(
        Expression<Func<TestSection, bool>>? filter = null,
        Func<IQueryable<TestSection>, IOrderedQueryable<TestSection>>? orderBy = null,
        string? includeProperties = "");

    Task<IList<TestSectionDto>> FindAllWithConditionAndThenIncludeAsync(
        Expression<Func<TestSection, bool>>? filter = null,
        Func<IQueryable<TestSection>, IOrderedQueryable<TestSection>>? orderBy = null,
        List<Func<IQueryable<TestSection>, IIncludableQueryable<TestSection, object>>>? includes = null);
    
    // Additional
    Task<IList<TestSectionDto>> FindAllByTestId(Guid testId);
}