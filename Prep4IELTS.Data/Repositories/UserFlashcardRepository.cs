using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data.Repositories;

public class UserFlashcardRepository : GenericRepository<UserFlashcard>
{
    public UserFlashcardRepository(Prep4IeltsContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<UserFlashcard?> FindByUserAndFlashcardIdAsync(int flashcardId, Guid userId)
    {
         return await _dbSet.FirstOrDefaultAsync(u => u.FlashcardId == flashcardId && u.UserId == userId);
    }

    public async Task<UserFlashcard?> FindUserPracticingProgressAsync(int flashcardId, Guid userId, bool? isTrackProgress)
    {
        var userFlashcard = await _dbSet
            .AsSplitQuery()
            .Include(u => u.UserFlashcardProgresses)
            .ThenInclude(ufp => ufp.FlashcardDetail)
            .ThenInclude(fd => fd.CloudResource)
            .FirstOrDefaultAsync(uf => uf.FlashcardId == flashcardId && uf.UserId == userId);

        if (userFlashcard != null && 
            isTrackProgress.HasValue && isTrackProgress.Value)
        {
            userFlashcard.UserFlashcardProgresses = userFlashcard.UserFlashcardProgresses
                // .OrderBy(ufp => ufp.ProgressStatus == FlashcardProgressStatus.New.GetDescription() ? 0 :
                //                 ufp.ProgressStatus == FlashcardProgressStatus.Studying.GetDescription() ? 1 :
                //                 ufp.ProgressStatus == FlashcardProgressStatus.Proficient.GetDescription() ? 2 :
                //                 ufp.ProgressStatus == FlashcardProgressStatus.Starred.GetDescription() ? 3 : 4)
                // Only get new and starred progress status even if tracking progress 
                .Where(ufp => ufp.ProgressStatus == FlashcardProgressStatus.New.GetDescription()
                    || ufp.ProgressStatus == FlashcardProgressStatus.Starred.GetDescription())
                .ToList();
        }

        return userFlashcard;
    }
    
    public async Task<UserFlashcard?> FindUserPracticingProgressWithStatusAsync(
        int flashcardId, Guid userId, List<FlashcardProgressStatus> statuses)
    {
        var userFlashcard = await _dbSet
            .AsSplitQuery()
            .Include(u => u.UserFlashcardProgresses) 
            .ThenInclude(ufp => ufp.FlashcardDetail)
            .ThenInclude(fd => fd.CloudResource)
            .FirstOrDefaultAsync(
                uf => uf.FlashcardId == flashcardId && uf.UserId == userId);
        
        // If userFlashcard is found, filter the progress statuses 
        if (userFlashcard != null)
        {
            var progressStatusStr = statuses.Select(s => s.GetDescription()).ToList();
            userFlashcard.UserFlashcardProgresses = userFlashcard.UserFlashcardProgresses
                .Where(ufp => progressStatusStr.Contains(ufp.ProgressStatus))
                .OrderBy(ufp => ufp.ProgressStatus == FlashcardProgressStatus.New.GetDescription() ? 0 :
                    ufp.ProgressStatus == FlashcardProgressStatus.Studying.GetDescription() ? 1 :
                    ufp.ProgressStatus == FlashcardProgressStatus.Proficient.GetDescription() ? 2 :
                    ufp.ProgressStatus == FlashcardProgressStatus.Starred.GetDescription() ? 3 : 4)
                .ToList();
        }

        return userFlashcard;
    }

    public async Task ResetFlashcardProgressAsync(int flashcardId, Guid userId)
    {
        var userFlashcardProgresses = await DbContext.UserFlashcardProgresses
            .AsSplitQuery()
            .Where(uf => uf.UserFlashcard.FlashcardId == flashcardId 
                                       && uf.UserFlashcard.UserId == userId)
            .ToListAsync();
        
        // Reset flashcard progress status to [NEW]
        foreach (var ufg in userFlashcardProgresses)
        {
            ufg.ProgressStatus = FlashcardProgressStatus.New.GetDescription();
        }
    }
}
