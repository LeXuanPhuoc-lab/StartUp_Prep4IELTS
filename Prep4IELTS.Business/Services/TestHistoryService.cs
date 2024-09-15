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
        Expression<Func<TestHistory, bool>>? filter, 
        Func<IQueryable<TestHistory>, IOrderedQueryable<TestHistory>>? orderBy = null, 
        string? includeProperties = "")
    {
        var testHistoryEntity = 
            await unitOfWork.TestHistoryRepository.FindOneWithConditionAsync(filter, null, includeProperties);
        return testHistoryEntity.Adapt<TestHistoryDto>();
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
    
    // Additional
    public async Task<IList<TestHistoryDto>> FindAllByTestAndUserAsync(Guid testId, Guid userId)
    {
        var testHistoryEntities = 
            await unitOfWork.TestHistoryRepository.FindAllByTestAndUserAsync(testId, userId);
        return testHistoryEntities.Adapt<List<TestHistoryDto>>(); 
    }

    public async Task<IList<TestHistoryDto>> FindAllUserIdAsync(Guid userId)
    {
        var testHistoryEntities = 
            await unitOfWork.TestHistoryRepository.FindAllByUserIdAsync(userId);
        return testHistoryEntities.Adapt<List<TestHistoryDto>>(); 
    }

    public async Task<IList<TestHistoryDto>> FindAllByUserIdWithDaysRangeAsync(Guid userId, int days)
    {
        var testHistoryEntities = 
            await unitOfWork.TestHistoryRepository.FindAllByUserIdWithDaysRangeAsync(userId, days);
        return testHistoryEntities.Adapt<List<TestHistoryDto>>(); 
    }

    public async Task<TestHistoryDto> FindByIdWithIncludePartitionAndGradeAsync(int testHistoryId)
    {
        var testHistoryEntity = 
            await unitOfWork.TestHistoryRepository.FindByIdWithIncludePartitionAndGradeAsync(testHistoryId);
        return testHistoryEntity.Adapt<TestHistoryDto>();
    }

    public async Task<bool> RemoveAllByTestId(Guid testId)
    {
        return await unitOfWork.TestHistoryRepository.RemoveAllByTestId(testId);
    }

    public async Task<bool> IsExistTestHistoryAsync(int testHistoryId)
    {
        return await unitOfWork.TestHistoryRepository.IsExistTestHistoryAsync(testHistoryId);
    }
}