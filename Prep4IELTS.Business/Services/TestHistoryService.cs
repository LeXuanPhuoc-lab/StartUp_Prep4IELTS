using System.Linq.Expressions;
using Mapster;
using MapsterMapper;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class TestHistoryService(UnitOfWork unitOfWork) : ITestHistoryService
{
    public async Task<bool> InsertAsync(TestHistoryDto testHistory)
    {
        await unitOfWork.TestHistoryRepository.InsertAsync(testHistory.Adapt<TestHistory>());
        return await unitOfWork.TestHistoryRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        await unitOfWork.TestHistoryRepository.RemoveAsync(id);
        return await unitOfWork.TestHistoryRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task UpdateAsync(TestHistoryDto testHistory)
    {
        var testHistoryEntity = await unitOfWork.TestHistoryRepository.FindAsync(testHistory.TestHistoryId);

        if (testHistoryEntity == null) return;
        
        // Update properties here...
        
        await unitOfWork.TestHistoryRepository.UpdateAsync(testHistoryEntity);
        await unitOfWork.TestHistoryRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<TestHistoryDto> FindAsync(int id)
    {
        var testHistoryEntity = await unitOfWork.TestHistoryRepository.FindAsync(id);
        return testHistoryEntity.Adapt<TestHistoryDto>();
    }

    public async Task<IList<TestHistoryDto>> FindAllAsync()
    {
        var testHistoryEntities = await unitOfWork.TestHistoryRepository.FindAllAsync();
        return testHistoryEntities.Adapt<List<TestHistoryDto>>();
    }
    
    public async Task<TestHistoryDto> FindOneWithConditionAsync(
        Expression<Func<TestHistory, bool>> filter, 
        string? includeProperties = "")
    {
        var testHistoryEntities = 
            await unitOfWork.TestHistoryRepository.FindOneWithConditionAsync(filter, includeProperties);
        return testHistoryEntities.Adapt<TestHistoryDto>();
    }
    
    public async Task<IList<TestHistoryDto>> FindAllWithConditionAsync(
        Expression<Func<TestHistory, bool>>? filter = null, 
        Func<IQueryable<TestHistory>, IOrderedQueryable<TestHistory>>? orderBy = null, 
        string? includeProperties = "")
    {
        var testHistoryEntities = 
            await unitOfWork.TestHistoryRepository.FindAllWithConditionAsync(filter, orderBy, includeProperties);
        return testHistoryEntities.Adapt<List<TestHistoryDto>>();
    }
}