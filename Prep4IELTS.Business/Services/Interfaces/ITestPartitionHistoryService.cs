using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITestPartitionHistoryService
{
    Task<PartitionHistoryDto> FindByIdAndGradeAsync(int id, int testGradeId, bool? hasPremiumPackage);
}