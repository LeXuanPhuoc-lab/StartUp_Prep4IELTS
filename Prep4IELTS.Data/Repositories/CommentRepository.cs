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

    public async Task<IEnumerable<Comment>> FindAllWithSizeByTestIdAsync(Guid testId, int totalElementSize)
    {
        var commentEntities = await _dbSet.Where(x => 
                x.TestId != null && x.TestId.Equals(testId))
            .OrderByDescending(x => x.CommentDate)
            .Take(totalElementSize).ToListAsync();

        foreach (var comment in commentEntities)
        {
            await LoadInverseParentCommentRecursively(comment);
        }

        return commentEntities;
    }


    private async Task LoadInverseParentCommentRecursively(Comment comment)
    {
        await _dbSet.Entry(comment)
            .Collection(x => x.InverseParentComment)
            .LoadAsync();

        foreach (var childElement in comment.InverseParentComment)
        {
            await LoadInverseParentCommentRecursively(childElement);
        }
    }
    
}