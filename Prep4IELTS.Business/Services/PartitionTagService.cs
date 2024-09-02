using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class PartitionTagService(UnitOfWork unitOfWork) : IPartitionTagService
{
    public async Task<List<PartitionTagDto>> FindAllAsync()
    {
        var partitionTagEntities = await unitOfWork.PartitionTagRepository.FindAllAsync();
        return partitionTagEntities.Adapt<List<PartitionTagDto>>();
    }
}