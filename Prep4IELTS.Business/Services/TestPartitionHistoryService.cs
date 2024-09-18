using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class TestPartitionHistoryService(UnitOfWork unitOfWork) : ITestPartitionHistoryService
{
    public async Task<PartitionHistoryDto> FindByIdAndGradeAsync(int id, int testGradeId, bool? hasPremiumPackage)
    {
        var partitionHistoryEntity = 
            await unitOfWork.PartitionHistoryRepository.FindByIdAndGradeAsync(id, testGradeId, hasPremiumPackage);
        return partitionHistoryEntity.Adapt<PartitionHistoryDto>();
    }
}