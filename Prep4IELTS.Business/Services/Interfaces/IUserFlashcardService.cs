using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IUserFlashcardService
{
    Task<bool> InsertAsync(UserFlashcardDto userFlashcardDto);
}