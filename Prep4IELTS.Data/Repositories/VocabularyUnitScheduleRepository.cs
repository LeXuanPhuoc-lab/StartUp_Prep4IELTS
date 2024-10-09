using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class VocabularyUnitScheduleRepository : GenericRepository<VocabularyUnitSchedule>
{
    public VocabularyUnitScheduleRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<VocabularyUnitSchedule>> FindCalendarAsync(
        Guid userId,
        DateTime startDate, DateTime endDate)
    {
        return await _dbSet.AsSplitQuery()
            .Where(vus => 
                vus.UserFlashcard.UserId == userId &&
                vus.CreateDate.Date >= startDate.Date && vus.CreateDate.Date <= endDate.Date)
            .Include(vus => vus.FlashcardDetail)
            .ToListAsync();
    }
}