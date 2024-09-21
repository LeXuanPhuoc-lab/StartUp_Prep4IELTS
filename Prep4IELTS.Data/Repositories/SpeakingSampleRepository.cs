using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class SpeakingSampleRepository : GenericRepository<SpeakingSample>
{
    public SpeakingSampleRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }
    
    public async Task<IList<SpeakingSample>> FindAllWithConditionAndPagingAsync(
        Expression<Func<SpeakingSample, bool>>? filter,
        Func<IQueryable<SpeakingSample>, IOrderedQueryable<SpeakingSample>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize)
    {
        IQueryable<SpeakingSample> query = _dbSet.AsQueryable();

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

                // Add AsSplitQuery when includes are present
                query = query.AsSplitQuery();
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        else
        {
            query = query.OrderBy(x => x.SpeakingSampleId);
        }

        // Check whether pageIndex < 1
        if (!pageIndex.HasValue || pageIndex.Value < 1) pageIndex = 1;
        // Check whether pageSize < 1
        if (!pageSize.HasValue || pageSize.Value < 1) pageSize = 10;

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

    public async Task<SpeakingSample?> FindWithUserAsync(int speakingSampleId, Guid userId)
    {
        return await _dbSet.AsSplitQuery()
            .Include(ss => ss.SpeakingParts)
            .Include(ss => ss.UserSpeakingSampleHistories
                .Where(usph => usph.UserId == userId))
            .FirstOrDefaultAsync(ss => ss.SpeakingSampleId == speakingSampleId);
    }
    
    public async Task<bool> InsertUserSpeakingSampleHistoryAsync(int speakingSampleId, Guid userId)
    {
        // Find existing speaking sample
        var existingSpeakingSample = 
            await _dbSet.FirstOrDefaultAsync(ss => ss.SpeakingSampleId == speakingSampleId);

        if (existingSpeakingSample != null)
        {
            // Initiate user speaking sample history
            existingSpeakingSample.UserSpeakingSampleHistories.Add(new UserSpeakingSampleHistory()
            {
                SpeakingSampleId = speakingSampleId,
                UserId = userId
            });
            
            // Save change db
            return await SaveChangeWithTransactionAsync() > 0;
        }

        return false;
    }

    public async Task<int> CountTotalAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<bool> IsExistUserSpeakingSampleHistoryAsync(int speakingSampleId, Guid userId)
    {
        return await DbContext.UserSpeakingSampleHistories.AnyAsync(x => 
            x.SpeakingSampleId == speakingSampleId && x.UserId == userId);
    }

    public async Task<List<UserSpeakingSampleHistory>> FindUserSpeakingSampleHistoryAsync(int speakingSampleId, Guid userId)
    {
        return await DbContext.UserSpeakingSampleHistories.Where(x => 
            x.SpeakingSampleId == speakingSampleId && x.UserId == userId).ToListAsync();
    }
}