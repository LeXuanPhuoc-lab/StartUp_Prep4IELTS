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

    public async Task RemoveByDeActiveAsync(Guid userId)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.UserId == userId);
        
        if(user == null) throw new NullReferenceException("User not found");
        
        // Remove by de-active
        user.IsActive = false;
    }

    // Task<bool> RemoveAsync(Guid userId)
    // {
    //     var toDeleteUser = _dbSet
    //         // .Include(u => u.Comments)
    //         // .Include(u => u.TestHistories)
    //         
    //         .FirstOrDefaultAsync(u => u.UserId == userId);
    // }
}