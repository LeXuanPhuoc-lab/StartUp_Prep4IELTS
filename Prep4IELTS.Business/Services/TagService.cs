using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class TagService(UnitOfWork unitOfWork) : ITagService
{
    public async Task<TagDto> FindByIdAsync(int tagId)
    {
        var tagEntity =  await unitOfWork.TagRepository.FindAsync(tagId);
        return tagEntity.Adapt<TagDto>();
    }

    public async Task<List<TagDto>> FindAllAsync()
    {
        var tagEntities =  await unitOfWork.TagRepository.FindAllAsync();
        return tagEntities.Adapt<List<TagDto>>();
    }
}