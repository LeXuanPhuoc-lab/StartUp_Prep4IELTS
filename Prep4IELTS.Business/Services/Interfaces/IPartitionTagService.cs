using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IPartitionTagService
{
    Task<List<PartitionTagDto>> FindAllAsync();
}