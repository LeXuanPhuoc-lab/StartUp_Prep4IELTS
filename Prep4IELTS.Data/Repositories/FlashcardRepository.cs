using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data.Repositories;

public class FlashcardRepository : GenericRepository<Flashcard>
{
    public FlashcardRepository(Prep4IeltsContext dbContext)
        : base(dbContext)
    {
    }

    public async Task InsertPrivacyAsync(Flashcard flashcard, Guid userId)
    {
        // Check exist user id
        var existUserId = await DbContext.Users.AnyAsync(x => x.UserId == userId);
        
        // Return <- Not exist user or flashcard 
        if(!existUserId) return;

        // Initiate list of user flashcard progress
        var userFlashcardProgresses = new List<UserFlashcardProgress>();
        // Iterate all flashcard detail in flashcard
        if (flashcard.FlashcardDetails != null!)
        {
            foreach (var fcDetail in flashcard.FlashcardDetails)
            {
                userFlashcardProgresses.Add(new()
                {
                    // Default progress status
                    ProgressStatus = FlashcardProgressStatus.New.GetDescription(),
                    FlashcardDetailId = fcDetail.FlashcardDetailId
                });
            }
        }
        
        
        // Progress add new user flashcard 
        flashcard.UserFlashcards = new List<UserFlashcard>()
        {
            new()
            {
                UserId = userId,
                UserFlashcardProgresses = userFlashcardProgresses
            }
        };
        
        // Add new 
        DbContext.Flashcards.Add(flashcard);
    }
    
    public async Task<bool> IsExistUserFlashcardAsync(int flashcardId, Guid userId)
    {
        return await DbContext.UserFlashcards.AnyAsync(uf => 
            uf.FlashcardId == flashcardId && uf.UserId == userId);
    }
    
    public async Task<bool> IsExistAsync(int flashcardId)
    {
        return await _dbSet.AnyAsync(f => f.FlashcardId == flashcardId);
    }

    public async Task<bool> IsPublicAsync(int flashcardId)
    {
        return await DbContext.Flashcards.AnyAsync(f => f.FlashcardId == flashcardId && f.IsPublic);
    }
    
    public async Task<int> CountTotalAsync()
    {
        return await _dbSet.CountAsync();
    }
    
    public async Task<IList<Flashcard>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Flashcard, bool>>? filter,
        Func<IQueryable<Flashcard>, IOrderedQueryable<Flashcard>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize)
    {
        IQueryable<Flashcard> query = _dbSet.AsQueryable();

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
            query = query.OrderBy(x => x.FlashcardId);
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

    public async Task<UserFlashcard?> FindUserFlashcardAsync(int flashcardId, Guid userId)
    {
        return await DbContext.UserFlashcards
            .Include(uf => uf.User)
            .FirstOrDefaultAsync(f =>
                f.FlashcardId == flashcardId && f.UserId == userId);
    }
    
    public async Task<Flashcard?> FindByIdAsync(int flashcardId)
    {
        return await _dbSet
            .AsSplitQuery()
            .Where(f => f.IsPublic)
            .Include(f => f.FlashcardDetails)
                .ThenInclude(fd => fd.CloudResource)
            .FirstOrDefaultAsync(x => x.FlashcardId == flashcardId);
    }
    
    public async Task<Flashcard?> FindByIdAsync(int flashcardId, Guid userId)
    {
        return await _dbSet
            .AsSplitQuery()
            .Include(f => f.FlashcardDetails)
                .ThenInclude(fd => fd.CloudResource)
            .Include(f => f.UserFlashcards.Where(x => x.UserId == userId))
                .ThenInclude(uf => uf.UserFlashcardProgresses)
            .FirstOrDefaultAsync(x => x.FlashcardId == flashcardId);
    }

    public override async Task RemoveAsync(object id)
    {
        var keyTypeofNum = int.TryParse(id.ToString(), out int flashcardId);
        if(!keyTypeofNum) throw new ArgumentException("Not valid type of flashcardId, key integer is required");
        
        var flashcardEntity = await _dbSet
            .AsSplitQuery()
            .Include(f => f.FlashcardDetails)
                .ThenInclude(f => f.CloudResource)
            .Include(f => f.UserFlashcards)
            .FirstOrDefaultAsync(x => x.FlashcardId == flashcardId);
        
        // Check exist flashcard 
        if (flashcardEntity == null) return;
        
        // Check if any user hold flashcard
        var existUser = flashcardEntity.UserFlashcards.Any();
        if (!existUser)
        {
            // Progress remove flashcard
            _dbSet.Remove(flashcardEntity);
        }
    }

    public async Task RemoveUserFlashcardAsync(int flashcardId, Guid userId)
    {
        var flashcardEntity = await _dbSet.FirstOrDefaultAsync(x =>
            x.FlashcardId == flashcardId);
        if (flashcardEntity == null) return;

        UserFlashcard? userFlashcard;

        // Remove progress & flashcard
        userFlashcard = await DbContext.UserFlashcards
            .Include(uf => uf.UserFlashcardProgresses)
            .Include(uf => uf.Flashcard)
                .ThenInclude(f => f.FlashcardDetails)
                .ThenInclude(f => f.CloudResource)
            .Include(uf => uf.VocabularyUnitSchedules)
            .FirstOrDefaultAsync(x => x.FlashcardId == flashcardId && x.UserId == userId);
        
        if (userFlashcard != null)
        {
            if (DbContext.Entry(userFlashcard).State == EntityState.Detached)
            {
                DbContext.Attach(userFlashcard);
            }
            DbContext.UserFlashcards.Remove(userFlashcard);
            
            // Perform remove flashcard if privacy
            if(!flashcardEntity.IsPublic) DbContext.Flashcards.Remove(userFlashcard.Flashcard);
        }
    }
    
    public async Task AddFlashcardToUserAsync(int flashcardId, Guid userId)
    {
        // Check exist user id
        var existUserId = await DbContext.Users.AnyAsync(x => x.UserId == userId);
        
        // Get flashcard by id
        var flashcardEntity  = await _dbSet
            .AsSplitQuery()
            .Include(f => f.FlashcardDetails)
            .FirstOrDefaultAsync(x => x.FlashcardId == flashcardId);
        
        // Return <- Not exist user or flashcard 
        if(!existUserId || flashcardEntity == null) return;

        // Initiate list of user flashcard progress
        var userFlashcardProgresses = new List<UserFlashcardProgress>();
        
        // Iterate all flashcard detail in flashcard
        foreach (var fcDetail in flashcardEntity.FlashcardDetails)
        {
            userFlashcardProgresses.Add(new()
            {
                // Default progress status
                ProgressStatus = FlashcardProgressStatus.New.GetDescription(),
                FlashcardDetailId = fcDetail.FlashcardDetailId
            });
        }
        
        // Progress add new user flashcard 
        var userFlashcard = new UserFlashcard()
        {
            FlashcardId = flashcardId,
            UserId = userId,
            UserFlashcardProgresses = userFlashcardProgresses
        };
        
        // Add new 
        DbContext.UserFlashcards.Add(userFlashcard);
    }
    

    public async Task UpdateUserFlashcardProgressStatusAsync(
        int userFlashcardProgressId, FlashcardProgressStatus status)
    {
        var userFlashcardProgress = await DbContext.UserFlashcardProgresses
            .FirstOrDefaultAsync(x => 
                x.UserFlashcardProgressId == userFlashcardProgressId);

        if (userFlashcardProgress == null) return;
        
        userFlashcardProgress.ProgressStatus = status.GetDescription();
    }

    public async Task UpdateFlashcardTotalViewAsync(int flashcardId)
    {
        var flashcardEntity = await _dbSet.FirstOrDefaultAsync(x => x.FlashcardId == flashcardId);
        if (flashcardEntity == null) return;
            
        // Increase total view 
        flashcardEntity.TotalView++;
    }
    
    public async Task<bool> PublishAsync(int flashcardId)
    {
        var flashcard = await _dbSet.FirstOrDefaultAsync(f => f.FlashcardId == flashcardId);

        if(flashcard == null) return false;
        flashcard.IsPublic = true;

        return await SaveChangeWithTransactionAsync() > 0;
    }
    
    public async Task<bool> HideAsync(int flashcardId)
    {
        var flashcard = await _dbSet.FirstOrDefaultAsync(f => f.FlashcardId == flashcardId);

        if(flashcard == null) return false;
        flashcard.IsPublic = false;

        return await SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> IsAlreadyPublishedAsync(int flashcardId)
    {
        return await _dbSet.AnyAsync(f => f.FlashcardId == flashcardId && f.IsPublic);
    }

    public async Task<(int totalNew, int totalStudying, int totalProficient, int totalStarred)> CalculateAllUserFlashcardProgressAsync(Guid userId)
    {
        // Get user flashcard by id and user id
        var userFlashcardEntity = await DbContext.UserFlashcards.Where(x => x.UserId == userId).ToListAsync();
        
        if(!userFlashcardEntity.Any()) return (0, 0, 0,0);
        
        // Select list of user flashcard id
        var userFlashcardIds = userFlashcardEntity.Select(x => x.UserFlashcardId).ToList();
        
        // Query the UserFlashcardProgresses and group by ProgressStatus
        var progressCounts = await DbContext.UserFlashcardProgresses
            .Where(ufp => userFlashcardIds.Contains(ufp.UserFlashcardId))
            .GroupBy(ufp => ufp.ProgressStatus)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count(),
            })
            .ToListAsync();
        
        // Map the results to the corresponding statuses
        var totalNew = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.New.GetDescription())?.Count ?? 0;
        var totalStudying = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.Studying.GetDescription())?.Count ?? 0;
        var totalProficient = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.Proficient.GetDescription())?.Count ?? 0;
        var totalStarred = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.Starred.GetDescription())?.Count ?? 0;
        
        return (totalNew, totalStudying, totalProficient, totalStarred);
    }
    
    public async Task<(int totalNew, int totalStudying, int totalProficient, int totalStarred)> CalculateUserFlashcardProgressAsync(
        int flashcardId, Guid userId)
    {
        // Get user flashcard by id and user id
        var userFlashcardEntity = await DbContext.UserFlashcards.FirstOrDefaultAsync(x => 
            x.FlashcardId == flashcardId && x.UserId == userId);
        
        if(userFlashcardEntity == null) return (0, 0, 0,0);
        
        // Query the UserFlashcardProgresses and group by ProgressStatus
        var progressCounts = await DbContext.UserFlashcardProgresses
            .Where(ufp => ufp.UserFlashcardId == userFlashcardEntity.UserFlashcardId)
            .GroupBy(ufp => ufp.ProgressStatus)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count(),
            })
            .ToListAsync();
        
        // Map the results to the corresponding statuses
        var totalNew = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.New.GetDescription())?.Count ?? 0;
        var totalStudying = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.Studying.GetDescription())?.Count ?? 0;
        var totalProficient = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.Proficient.GetDescription())?.Count ?? 0;
        var totalStarred = progressCounts.FirstOrDefault(p => p.Status == FlashcardProgressStatus.Starred.GetDescription())?.Count ?? 0;
        
        return (totalNew, totalStudying, totalProficient, totalStarred);
    }
}