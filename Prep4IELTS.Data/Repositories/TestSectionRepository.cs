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
}