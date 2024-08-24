using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class TestHistoryRepository : GenericRepository<TestHistory>
{
    public TestHistoryRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<IList<TestHistory>> FindAllByTestAndUserAsync(Guid testId, Guid userId)
    {
        return await _dbSet
            .Where(x =>
                x.TestId.ToString().Equals(testId.ToString()) &&
                x.UserId.ToString().Equals(userId.ToString()))
            .Select(th => new TestHistory()
            {
                TestHistoryId = th.TestHistoryId,
                TotalRightAnswer = th.TotalRightAnswer,
                TotalWrongAnswer = th.TotalWrongAnswer,
                TotalSkipAnswer = th.TotalSkipAnswer,
                TotalQuestion = th.TotalQuestion,
                TotalCompletionTime = th.TotalCompletionTime,
                TakenDate = th.TakenDate,
                AccuracyRate = th.AccuracyRate,
                IsFull = th.IsFull,
                TestType = th.TestType,
                BandScore = th.BandScore
            }).ToListAsync();
    }
}