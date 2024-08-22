using System.Linq.Expressions;
using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class TestService : ITestService
{
    private readonly UnitOfWork _unitOfWork;

    public TestService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> InsertAsync(TestDto test)
    {
        await _unitOfWork.TestRepository.InsertAsync(test.Adapt<Test>());
        return await _unitOfWork.TestRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        await _unitOfWork.TestRepository.RemoveAsync(id);
        return await _unitOfWork.TestRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task UpdateAsync(TestDto test)
    {
        var testEntity = await _unitOfWork.TestRepository.FindAsync(test.TestId);

        if (testEntity == null) return;
        
        // Update properties here...
        
        await _unitOfWork.TestRepository.UpdateAsync(testEntity);
        await _unitOfWork.TestRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<TestDto> FindAsync(Guid id)
    {
        var testEntity = await _unitOfWork.TestRepository.FindAsync(id);
        return testEntity.Adapt<TestDto>();
    }

    public async Task<IList<TestDto>> FindAllAsync()
    {
        var testEntities = await _unitOfWork.TestRepository.FindAllAsync();
        return testEntities.Adapt<List<TestDto>>();
    }

    public async Task<IList<TestDto>> FindWithConditionAsync(Expression<Func<Test, bool>>? filter = null, 
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy = null, 
        string? includeProperties = "")
    {
        var testEntities = 
            await _unitOfWork.TestRepository.FindWithConditionAsync(filter, orderBy, includeProperties);
        return testEntities.Adapt<List<TestDto>>();
    }
}