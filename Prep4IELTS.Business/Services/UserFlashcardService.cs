using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class UserFlashcardService(UnitOfWork unitOfWork) : IUserFlashcardService
{
    public async Task<bool> InsertAsync(UserFlashcardDto userFlashcardDto)
    {
        await unitOfWork.UserFlashcardRepository.InsertAsync(userFlashcardDto.Adapt<UserFlashcard>());
        return await unitOfWork.UserFlashcardRepository.SaveChangeWithTransactionAsync() > 0;
    }
}