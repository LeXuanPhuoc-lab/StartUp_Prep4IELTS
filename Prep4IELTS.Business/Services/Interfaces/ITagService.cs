using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITagService
{
    Task<TagDto> FindByIdAsync(int tagId);
    Task<List<TagDto>> FindAllAsync();
    Task<bool> RemoveAllTestTag(Guid testId);
}