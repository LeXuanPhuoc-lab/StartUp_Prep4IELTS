using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class FlashcardDetailService(UnitOfWork unitOfWork) : IFlashcardDetailService
{
    public async Task<bool> InsertAsync(int flashcardId, FlashcardDetailDto flashcardDetailDto)
    {
        // Create new flashcard detail
        await unitOfWork.FlashcardDetailRepository.InsertAsync(
            flashcardId, flashcardDetailDto.Adapt<FlashcardDetail>());
        // Save change DB
        return await unitOfWork.FlashcardDetailRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> InsertPrivacyAsync(int flashcardId, Guid userId, FlashcardDetailDto flashcardDetailDto)
    {
        // Create new flashcard detail
        await unitOfWork.FlashcardDetailRepository.InsertPrivacyAsync(
            flashcardId, userId, flashcardDetailDto.Adapt<FlashcardDetail>());
        
        // Save change DB
        return await unitOfWork.FlashcardDetailRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveAsync(int flashcardDetailId)
    {
        // Remove flashcard detail
        await unitOfWork.FlashcardDetailRepository.RemoveAsync(flashcardDetailId);
        // Save change DB
        return await unitOfWork.FlashcardDetailRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> IsInPublicFlashcard(int flashcardDetailId)
    {
        return await unitOfWork.FlashcardDetailRepository.IsInPublicFlashcard(flashcardDetailId);
    }

    public async Task UpdateAsync(FlashcardDetailDto flashcardDetailDto)
    {
        // Get flashcard detail by id
        var flashcardDetailEntity = await unitOfWork.FlashcardDetailRepository
            .FindOneWithConditionAsync(fd => fd.FlashcardDetailId == flashcardDetailDto.FlashcardDetailId);

        // Check exist flashcard detail
        if (flashcardDetailEntity == null) return;
        
        // Update flashcard detail properties
        flashcardDetailEntity.WordText = flashcardDetailDto.WordText;
        flashcardDetailEntity.Definition = flashcardDetailDto.Definition;
        flashcardDetailEntity.WordForm = flashcardDetailDto.WordForm;
        flashcardDetailEntity.WordPronunciation = flashcardDetailDto.WordPronunciation;
        flashcardDetailEntity.Example = flashcardDetailDto.Example;
        flashcardDetailEntity.Description = flashcardDetailDto.Description;
        
        // Check exist update image 
        if (flashcardDetailDto.CloudResource != null 
            && flashcardDetailEntity.CloudResource != null)
        {
            // Update flashcard cloud resource properties
            flashcardDetailEntity.CloudResource.PublicId = flashcardDetailDto.CloudResource.PublicId;
            flashcardDetailEntity.CloudResource.Url = flashcardDetailDto.CloudResource.Url;
            flashcardDetailEntity.CloudResource.Bytes = flashcardDetailDto.CloudResource.Bytes;
            flashcardDetailEntity.CloudResource.ModifiedDate = DateTime.UtcNow;
        }
     
        await unitOfWork.FlashcardDetailRepository.UpdateAsync(flashcardDetailEntity);
        await unitOfWork.FlashcardDetailRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<FlashcardDetailDto?> FindByIdAsync(int flashcardDetailId)
    {
        var flashcardDetailEntity = await unitOfWork.FlashcardDetailRepository
            .FindByIdAsync(flashcardDetailId);
        return flashcardDetailEntity.Adapt<FlashcardDetailDto>();
    }
}