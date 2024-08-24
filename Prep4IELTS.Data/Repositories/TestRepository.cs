using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class TestRepository : GenericRepository<Test>
{
    public TestRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }
    
    public async Task<IEnumerable<Test>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Test, bool>>? filter,
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy,
        string? includeProperties, int? pageIndex, int? pageSize)
    {
        IQueryable<Test> query = _dbSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(
                         new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        
        // Check whether pageIndex < 1
        if (!pageIndex.HasValue || pageIndex.Value < 1) pageIndex = 1;
        // Check whether pageSize < 1
        if (!pageSize.HasValue || pageIndex.Value < 1) pageSize = 10;
        
        // Count offset
        var skipOffset = (pageIndex.Value - 1) * pageSize.Value;
        
        var result = await query
            // Skip elements
            .Skip(skipOffset)
            // Take total page elements
            .Take(pageSize.Value)
            // Convert to List
            .ToListAsync();

        // If result is empty and pageIndex > 1, reset to first page
        if (!result.Any() && pageIndex > 1)
        {
            result = await query.Take(pageSize.Value).ToListAsync();
        }

        return result;
    }

    public async Task<int> CountTotalAsync()
    {
        return await _dbSet.CountAsync();
    }
}