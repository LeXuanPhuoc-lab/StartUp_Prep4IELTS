using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class UserRepository : GenericRepository<User>
{
    public UserRepository(Prep4IeltsContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<bool> IsExistUserAsync(Guid userId)
    {
        return await _dbSet.AnyAsync(x => x.UserId.Equals(userId));
    }

    public async Task<bool> IsExistUsername(string username)
    {
        return await _dbSet.AnyAsync(x => x.Username.Equals(username));
    }

    public async Task<int> CountTotalAsync()
    {
        return await _dbSet.CountAsync();
    }
    
    public async Task<int> CountTotalActiveAsync()
    {
        return await _dbSet.Where(u => u.IsActive == true).CountAsync();
    }
    
    public async Task<int> CountTotalInActiveAsync()
    {
        return await _dbSet.Where(u => u.IsActive == false).CountAsync();
    }
    
    public async Task<User?> FindUserByClerkId(string clerkId)
    {
        return await _dbSet
            .Where(u => u.ClerkId == clerkId)
            .Include(u => u.Role)
            .SingleOrDefaultAsync();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<IEnumerable<User>> FindAllInActiveAsync()
    {
        return await _dbSet.Where(u => u.IsActive == false).ToListAsync();
    }

    public async Task<IList<User>> FindAllWithConditionAndPagingAsync(
        Expression<Func<User, bool>>? filter,
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize)
    {
        IQueryable<User> query = _dbSet.AsQueryable();

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
            query = query.OrderBy(x => x.Id);
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
    
    public async Task RemoveByDeActiveAsync(Guid userId)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.UserId == userId);
        
        if(user == null) throw new NullReferenceException("User not found");
        
        // Remove by de-active
        user.IsActive = false;
    }

    public async Task RemoveAsync(Guid userId)
    {
        // Remove all comment
        var userComments = 
            await DbContext.Comments.Where(x => x.UserId == userId).ToListAsync();
        if(userComments.Any()) DbContext.Comments.RemoveRange(userComments);
        // Remove all transaction 
        var userPremiumPackages = 
            await DbContext.UserPremiumPackages
                // Include user transactions 
                .Include(x => x.Transactions)
                .Where(x => x.UserId == userId).ToListAsync();  
        if(userPremiumPackages.Any()) DbContext.UserPremiumPackages.RemoveRange(userPremiumPackages);
        // Remove all user flashcard
        var userFlashcards = 
            await DbContext.UserFlashcards.Where(x => x.UserId == userId).ToListAsync();
        if(userFlashcards.Any()) DbContext.UserFlashcards.RemoveRange(userFlashcards);
        // Remove all user speaking sample history
        var userSpeakingSampleHistories = 
            await DbContext.UserSpeakingSampleHistories.Where(x => x.UserId == userId).ToListAsync();   
        if(userSpeakingSampleHistories.Any()) DbContext.UserSpeakingSampleHistories.RemoveRange(userSpeakingSampleHistories);
        
        // Remove user
        var toDeleteUser = await _dbSet.FindAsync(userId);

        if (toDeleteUser == null) return;
        
        if (DbContext.Users.Entry(toDeleteUser).State == EntityState.Detached)
        {
            DbContext.Attach(toDeleteUser);
        }
        // Perform remove 
        _dbSet.Remove(toDeleteUser);
    }
}