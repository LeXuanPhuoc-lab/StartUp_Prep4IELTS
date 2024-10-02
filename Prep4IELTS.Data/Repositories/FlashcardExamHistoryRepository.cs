using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class FlashcardExamHistoryRepository : GenericRepository<FlashcardExamHistory>
{
    public FlashcardExamHistoryRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<FlashcardExamHistory?> FindByUserFlashcardIdAtTakenDateAsync(
        int userFlashcardId, DateTime takenDateTime)
    {
        return await _dbSet
                .Include(feh => feh.FlashcardExamGrades)
                .ThenInclude(feg => feg.FlashcardDetail)
                .ThenInclude(fd => fd.CloudResource)
                .FirstOrDefaultAsync(feh => feh.UserFlashcardId == userFlashcardId 
                                                    && feh.TakenDate == takenDateTime);
    }
}