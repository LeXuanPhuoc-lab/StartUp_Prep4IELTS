using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data.Repositories;

public class FlashcardDetailRepository : GenericRepository<FlashcardDetail>
{
    public FlashcardDetailRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task InsertAsync(int flashcardId, FlashcardDetail flashcardDetail)
    {
        var flashcardEntity = await DbContext.Flashcards
            .Include(f => f.FlashcardDetails)
                .ThenInclude(fd => fd.CloudResource)
            .FirstOrDefaultAsync(f => f.FlashcardId == flashcardId);

        if (flashcardEntity == null) return;
        
        flashcardEntity.FlashcardDetails.Add(flashcardDetail);
    }
    
    public async Task InsertPrivacyAsync(int flashcardId, Guid userId, FlashcardDetail flashcardDetail)
    {
        // Check exist user id
        var existUserId = await DbContext.Users.AnyAsync(x => x.UserId == userId);
        
        // Get flashcard by id
        var flashcardEntity  = await DbContext.Flashcards
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.FlashcardId == flashcardId);
        
        // Return <- Not exist user or flashcard 
        if(!existUserId || flashcardEntity == null) return;
        
        // Update flashcard total words
        flashcardEntity.TotalWords++;
        
        // Check exist user flashcard
        var userFlashcard = await DbContext.UserFlashcards
            .Include(uf => uf.UserFlashcardProgresses)
            .FirstOrDefaultAsync(x => 
            x.FlashcardId == flashcardId && x.UserId == userId);
        
        if (userFlashcard == null)
        {
            // Progress add new user flashcard 
            userFlashcard = new UserFlashcard()
            {
                FlashcardId = flashcardId,
                UserId = userId,
                UserFlashcardProgresses = new List<UserFlashcardProgress>()
                {
                    new()
                    {
                        // Default progress status
                        ProgressStatus = FlashcardProgressStatus.New.GetDescription(),
                        FlashcardDetail = flashcardDetail
                    }
                }
            };
        
            // Add new 
            DbContext.UserFlashcards.Add(userFlashcard);
        }
        
        // Set flashcard id for flashcard detail
        flashcardDetail.FlashcardId = flashcardId;
        // Update user flashcard progress
        userFlashcard.UserFlashcardProgresses.Add(new()
        {
            // Default progress status
            ProgressStatus = FlashcardProgressStatus.New.GetDescription(),
            FlashcardDetail = flashcardDetail
        });
    }

    public async Task RemoveAsync(int flashcardDetailId)
    {
        var flashcardDetailEntity = await _dbSet
            .Include(fd => fd.UserFlashcardProgresses)
            .FirstOrDefaultAsync(f => f.FlashcardDetailId == flashcardDetailId);
        
        if(flashcardDetailEntity == null) return;
        
        // Update flashcard total word
        var flashcardEntity = await DbContext.Flashcards.FirstOrDefaultAsync(f => 
            f.FlashcardId == flashcardDetailEntity.FlashcardId);
        if(flashcardEntity != null) flashcardEntity.TotalWords--; 
        
        _dbSet.Remove(flashcardDetailEntity);
    }

    public async Task<FlashcardDetail?> FindByIdAsync(int flashcardDetailId)
    {
        return await _dbSet
            .Include(fd => fd.CloudResource)
            .FirstOrDefaultAsync(f => f.FlashcardDetailId == flashcardDetailId);
    }

    public async Task<bool> IsInPublicFlashcard(int flashcardDetailId)
    {
        var flashcardDetailEntity = await _dbSet
            .AsSplitQuery()
            .Include(fd => fd.Flashcard)
            .FirstOrDefaultAsync(fd => fd.FlashcardDetailId == flashcardDetailId);
        
        return flashcardDetailEntity is { Flashcard.IsPublic: true };
    }
}