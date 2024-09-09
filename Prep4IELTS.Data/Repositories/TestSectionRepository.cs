using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class TestSectionRepository : GenericRepository<TestSection>
{
    public TestSectionRepository(Prep4IeltsContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<IList<TestSection>> FindAllByTestId(Guid testId)
    {
        // return await _dbSet.AsSplitQuery()
        //     .Where(ts => 
        //         ts.TestId.ToString().Equals(testId.ToString()))
        //     .Include(ts => ts.TestSectionPartitions)
        //         .ThenInclude(tsp => tsp.PartitionTag).ToListAsync();
        
        return await _dbSet.AsSplitQuery()
            .Where(ts => 
                ts.TestId.ToString().Equals(testId.ToString()))
            .Select(ts => new TestSection()
            {
                TestSectionId = ts.TestSectionId,
                TestSectionName = ts.TestSectionName,
                TotalQuestion = ts.TotalQuestion,
                TestId = ts.TestId,
                TestSectionPartitions = ts.TestSectionPartitions.Select(tsp => new TestSectionPartition()
                {
                    TestSectionPartId = tsp.TestSectionPartId,
                    IsVerticalLayout = tsp.IsVerticalLayout,
                    TestSectionId = tsp.TestSectionId,
                    PartitionTagId = tsp.PartitionTagId,
                    PartitionTag = tsp.PartitionTag,
                }).ToList()
            })
            .ToListAsync();
    }

    // public async Task<bool> RemoveRangeTestSectionAndRelations(List<TestSection> testSections)
    // {
    //     foreach (var testSection in testSections)
    //     {
    //         var testSectionEntity = await _dbSet
    //             .AsSplitQuery()
    //             .Include(ts => ts.CloudResource)
    //             .Include(ts => ts.TestSectionPartitions)
    //             .ThenInclude(tsp => tsp.Questions)
    //             .ThenInclude(q => q.QuestionAnswers)
    //             .FirstOrDefaultAsync(ts => ts.TestSectionId == testSection.TestSectionId);
    //
    //         // if (testSectionEntity != null)
    //         // {
    //         //     var sectionPartitions = testSectionEntity.TestSectionPartitions.ToList();
    //         //     var partitionHistories = sectionPartitions.SelectMany(tsp => tsp.PartitionHistories).ToList();
    //         //     var testGrades = partitionHistories.SelectMany(tsp => tsp.TestGrades).ToList();
    //         //     var testHistories = partitionHistories.Select(ph => ph.TestHistory).ToList();
    //         //     var cloudResources = sectionPartitions.Select(sp => sp.CloudResource).ToList();
    //         //     var questions = sectionPartitions.SelectMany(sp => sp.Questions).ToList();
    //         //     var answers = questions.SelectMany(q => q.QuestionAnswers).ToList();
    //         //
    //         //     // Combine with cloud resource of section with section parts 
    //         //     cloudResources.Add(testSection.CloudResource);
    //         //     
    //         //     // Remove related entities
    //         //     DbContext.TestGrades.RemoveRange(testGrades);
    //         //     DbContext.PartitionHistories.RemoveRange(partitionHistories);
    //         //     DbContext.TestHistories.RemoveRange(testHistories);
    //         //     DbContext.CloudResources.RemoveRange(cloudResources);
    //         //     DbContext.QuestionAnswers.RemoveRange(answers);
    //         //     DbContext.Questions.RemoveRange(questions);
    //         //     DbContext.TestSectionPartitions.RemoveRange(sectionPartitions);   
    //         // }
    //         _dbSet.Remove(testSectionEntity);
    //     }
    //     
    //     return await SaveChangeWithTransactionAsync() > 0;
    // }
}