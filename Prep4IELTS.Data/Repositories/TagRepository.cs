using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class TagRepository : GenericRepository<Tag>
{
    public TagRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<bool> RemoveAllTestTag(Guid testId)
    {
        // Fetch the entities with their related tags
        var tags = await _dbSet
            .Include(x => x.Tests)
            .Where(x => x.Tests.Any(y => y.TestId == testId))
            .ToListAsync();

        // Remove the tags associated with the testId
        foreach (var tag in tags)
        {
            var testTags = tag.Tests.Where(y => y.TestId == testId).ToList();
            foreach (var testTag in testTags)
            {
                tag.Tests.Remove(testTag);
            }
        }

        // Mark the entities as modified
        foreach (var tag in tags)
        {
            DbContext.Entry(tag).State = EntityState.Modified;
        }

        // Save the changes
        var result = await SaveChangeAsync();

        // Detach all entities to stop tracking them
        foreach (var tag in tags)
        {
            DbContext.Entry(tag).State = EntityState.Detached;
        }

        return result > 0;
    }
}