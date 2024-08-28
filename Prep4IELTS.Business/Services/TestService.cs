using System.Linq.Expressions;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore.Query;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class TestService(
    UnitOfWork unitOfWork, 
    ITestHistoryService testHistoryService,
    ITestSectionService testSectionService) : ITestService
{
    // Basic
    public async Task<bool> InsertAsync(TestDto test)
    {
        await unitOfWork.TestRepository.InsertAsync(test.Adapt<Test>());
        return await unitOfWork.TestRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        await unitOfWork.TestRepository.RemoveAsync(id);
        return await unitOfWork.TestRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task UpdateAsync(TestDto test)
    {
        var testEntity = await unitOfWork.TestRepository.FindAsync(test.TestId);

        if (testEntity == null) return;
        
        // Update properties here...
        
        await unitOfWork.TestRepository.UpdateAsync(testEntity);
        await unitOfWork.TestRepository.SaveChangeWithTransactionAsync();
    }

    public async Task<TestDto> FindAsync(Guid id)
    {
        var testEntity = await unitOfWork.TestRepository.FindAsync(id);
        return testEntity.Adapt<TestDto>();
    }

    public async Task<IList<TestDto>> FindAllAsync()
    {
        var testEntities = await unitOfWork.TestRepository.FindAllAsync();
        return testEntities.Adapt<List<TestDto>>();
    }
    
    public async Task<TestDto> FindOneWithConditionAsync(
        Expression<Func<Test, bool>> filter, 
        string? includeProperties = "")
    {
        var testEntities = 
            await unitOfWork.TestRepository.FindOneWithConditionAsync(filter, includeProperties);
        return testEntities.Adapt<TestDto>();
    }
    
    public async Task<IList<TestDto>> FindAllWithConditionAsync(
        Expression<Func<Test, bool>>? filter = null, 
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy = null, 
        string? includeProperties = "")
    {
        var testEntities = 
            await unitOfWork.TestRepository.FindAllWithConditionAsync(filter, orderBy, includeProperties);
        return testEntities.Adapt<List<TestDto>>();
    }

    public async Task<IList<TestDto>> FindAllWithConditionAndThenIncludeAsync(
        Expression<Func<Test, bool>>? filter = null, 
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy = null, 
        List<Func<IQueryable<Test>, IIncludableQueryable<Test, object>>>? includes = null)
    {
        var testEntities = 
            await unitOfWork.TestRepository.FindAllWithConditionAndThenIncludeAsync(filter, orderBy, includes);
        return testEntities.Adapt<List<TestDto>>();
    }

    public async Task<IList<TestDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Test, bool>>? filter, 
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy, 
        string? includeProperties, 
        int? pageIndex, int? pageSize, 
        // Include test histories for user (if any)
        Guid? userId)
    {
        var testEntities = 
            await unitOfWork.TestRepository.FindAllWithConditionAndPagingAsync(
                filter, orderBy, includeProperties, pageIndex, pageSize);

        // Check whether user do the test
        var userTestHistories = await testHistoryService.FindAllWithConditionAsync(
            th => th.UserId.Equals(userId) &&
                  testEntities.Select(tst => tst.TestId.ToString()).Contains(th.TestId.ToString()));

        if (userTestHistories.Any()) // Check whether exist any test history for user
        {
            foreach (var tst in testEntities)
            {
                var historyDtos = userTestHistories
                    .Where(th => th.TestId.ToString().Equals(tst.TestId.ToString()))
                    .ToList();
                tst.TestHistories = historyDtos.Adapt<List<TestHistory>>();
                // Remove all test in test history
                foreach (var test in tst.TestHistories.Select(x => x.Test = null!))
                {
                }
            }
        }
        
        return testEntities.Adapt<List<TestDto>>();
    }
    
    // Additional
    public async Task<TestDto> FindByIdAsync(int id, Guid? userId)
    {
        var testEntity = await unitOfWork.TestRepository.FindOneWithConditionAsync(
            filter: x => x.Id == id,
            includeProperties: "Tags");
        if (testEntity == null) return null!; // Not found any match 
        
        // Find all test section
        var testSectionDtos = await testSectionService.FindAllByTestId(testEntity.TestId);
        if (testSectionDtos.Any()) testEntity.TestSections = testSectionDtos.Adapt<List<TestSection>>();
        
        // Find all test history by test and user 
        IList<TestHistoryDto> testHistoryDtos = null!;
        if (userId != null)
        {
            testHistoryDtos = await testHistoryService.FindAllByTestAndUserAsync(
                testEntity.TestId, Guid.Parse(userId.ToString()!));
            if(testHistoryDtos.Any()) testEntity.TestHistories = testHistoryDtos.Adapt<List<TestHistory>>();
        }

        return testEntity.Adapt<TestDto>();
    }

    public async Task<TestDto> FindByIdForPracticeAsync(int id, int[] sectionIds)
    {
        var testEntity = await unitOfWork.TestRepository.FindByIdForPracticeAsync(id, sectionIds);
        return testEntity.Adapt<TestDto>();
    }

    public async Task<TestDto> FindByIdForTestSimulationAsync(int id)
    {
        var testEntity = await unitOfWork.TestRepository.FindByIdForTestSimulationAsync(id);
        return testEntity.Adapt<TestDto>();
    }

    public async Task<TestDto> FindByIdAndGetAllAnswerAsync(int id)
    {
        var testEntity = await unitOfWork.TestRepository.FindByIdAndGetAllAnswerAsync(id);
        return testEntity.Adapt<TestDto>();
    }

    public async Task<int> CountTotalAsync()
    {
        return await unitOfWork.TestRepository.CountTotalAsync();
    }
}