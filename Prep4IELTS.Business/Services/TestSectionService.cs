using System.Linq.Expressions;
using Mapster;
using Microsoft.EntityFrameworkCore.Query;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class TestSectionService(UnitOfWork unitOfWork) : ITestSectionService
{
    public async Task<bool> InsertAsync(TestSectionDto testSection)
    {
        await unitOfWork.TestSectionRepository.InsertAsync(testSection.Adapt<TestSection>());
        return await unitOfWork.TestSectionRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveAsync(int id)
    {
        await unitOfWork.TestSectionRepository.RemoveAsync(id);
        return await unitOfWork.TestSectionRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task UpdateAsync(TestSectionDto testSection)
    {
        var testSectionEntity = await unitOfWork.TestSectionRepository.FindAsync(testSection.TestId);

        if (testSectionEntity == null) return;
        
        // Update properties here...
        
        await unitOfWork.TestSectionRepository.UpdateAsync(testSectionEntity);
        await unitOfWork.TestSectionRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<TestSectionDto> FindAsync(int id)
    {
        var testSectionEntity = await unitOfWork.TestSectionRepository.FindAsync(id);
        return testSectionEntity.Adapt<TestSectionDto>();
    }

    public async Task<IList<TestSectionDto>> FindAllAsync()
    {
        var testSectionEntities = await unitOfWork.TestSectionRepository.FindAllAsync();
        return testSectionEntities.Adapt<List<TestSectionDto>>();
    }
    
    public async Task<TestSectionDto> FindOneWithConditionAsync(
        Expression<Func<TestSection, bool>> filter, 
        string? includeProperties = "")
    {
        var testSectionEntities = 
            await unitOfWork.TestSectionRepository.FindOneWithConditionAsync(filter, includeProperties);
        return testSectionEntities.Adapt<TestSectionDto>();
    }
    
    public async Task<IList<TestSectionDto>> FindAllWithConditionAsync(
        Expression<Func<TestSection, bool>>? filter = null, 
        Func<IQueryable<TestSection>, IOrderedQueryable<TestSection>>? orderBy = null, 
        string? includeProperties = "")
    {
        var testSectionEntities = 
            await unitOfWork.TestSectionRepository.FindAllWithConditionAsync(filter, orderBy, includeProperties);
        return testSectionEntities.Adapt<List<TestSectionDto>>();
    }

    public async Task<IList<TestSectionDto>> FindAllWithConditionAndThenIncludeAsync(
        Expression<Func<TestSection, bool>>? filter = null, 
        Func<IQueryable<TestSection>, IOrderedQueryable<TestSection>>? orderBy = null, 
        List<Func<IQueryable<TestSection>, IIncludableQueryable<TestSection, object>>>? includes = null)
    {
        var testSectionEntities =
            await unitOfWork.TestSectionRepository.FindAllWithConditionAndThenIncludeAsync(filter, orderBy, includes);
        return testSectionEntities.Adapt<List<TestSectionDto>>();
    }
}