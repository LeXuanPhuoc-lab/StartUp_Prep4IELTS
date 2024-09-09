using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
            .Query()
            .Include(x => x.User)
            .LoadAsync();

        foreach (var childElement in comment.InverseParentComment)
        {
            await LoadInverseParentCommentRecursively(childElement);
        }
    }


    public async Task<bool> RemoveRangeCommentAndChildrenAsync(List<Comment> comments)
    {
        foreach (var cmt in comments)
        {
            var commentEntity = await _dbSet
                .FirstOrDefaultAsync(x => x.CommentId == cmt.CommentId);

            if (commentEntity != null)
            {
                await RemoveInverseParentCommentRecursively(commentEntity);
            }
        }

        return await SaveChangeWithTransactionAsync() > 0;
    }
    
    private async Task RemoveInverseParentCommentRecursively(Comment comment)
    {
        await _dbSet.Entry(comment)
            .Collection(x => x.InverseParentComment)
            .LoadAsync();

        foreach (var childElement in comment.InverseParentComment)
        {
            await RemoveInverseParentCommentRecursively(childElement); 
            childElement.InverseParentComment.Clear();
        }

        _dbSet.Remove(comment);
    }
    
    public async Task<IEnumerable<Comment>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Comment, bool>>? filter,
        Func<IQueryable<Comment>, IOrderedQueryable<Comment>>? orderBy,
        string? includeProperties, int? pageIndex, int? pageSize)
    {
        IQueryable<Comment> query = _dbSet.AsQueryable();

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
            query = query.OrderBy(x => x.CommentDate);
        }

        // Check whether pageIndex < 1
        if (!pageIndex.HasValue || pageIndex.Value < 1) pageIndex = 1;
        // Check whether pageSize < 1
        if (!pageSize.HasValue || pageSize.Value < 1) pageSize = 10;

        // Count offset
        var skipOffset = (pageIndex.Value - 1) * pageSize.Value;

        var results = await query
            // Skip elements
            .Skip(skipOffset)
            // Take total page elements
            .Take(pageSize.Value)
            // Convert to List
            .ToListAsync();
        
        // If result is empty and pageIndex > 1, reset to first page
        if (!results.Any() && pageIndex > 1)
        {
            results = await query.Take(pageSize.Value).ToListAsync();
        }
        
        foreach (var comment in results)
        {
            await LoadInverseParentCommentRecursively(comment);
        }

        return results;
    }

    public async Task<int> CountTotalByTestId(Guid testId)
    {
        return await _dbSet.CountAsync(x => x.TestId == testId);
    }
    
}