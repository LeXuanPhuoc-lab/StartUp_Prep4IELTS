using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class CommentRepository : GenericRepository<Comment>
{
    public CommentRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<Comment>> GetAllByTestIdAsync(Guid testId)
    {
        return await _dbSet.Where(x => 
                x.TestId != null && x.TestId.ToString()!.Equals(testId.ToString()))
            .Include(x => x.InverseParentComment).ToListAsync();
    }
}