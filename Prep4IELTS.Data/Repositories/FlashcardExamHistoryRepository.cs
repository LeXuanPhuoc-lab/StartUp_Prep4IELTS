using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using System.Linq.Expressions;

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

    public async Task<List<FlashcardExamHistory>> FindAllByUserAsync(
        Expression<Func<FlashcardExamHistory, bool>>? filter,
        Func<IQueryable<FlashcardExamHistory>, IOrderedQueryable<FlashcardExamHistory>>? orderBy,
        Guid userId)
    {
        var flashcardExamHistoriesQueryale = _dbSet
                .AsSplitQuery()
                .Where(feh => feh.UserFlashcard.UserId == userId)
                //.Include(feh => feh.FlashcardExamGrades)
                //.ThenInclude(feg => feg.FlashcardDetail)
                //.ThenInclude(fd => fd.CloudResource)
                .AsQueryable(); // Mark as building query

        if(filter != null)
        {
            flashcardExamHistoriesQueryale = flashcardExamHistoriesQueryale.Where(filter);
        }

        if(orderBy != null)
        {
            flashcardExamHistoriesQueryale = orderBy(flashcardExamHistoriesQueryale);
        }

        return await flashcardExamHistoriesQueryale.ToListAsync();
    }
}