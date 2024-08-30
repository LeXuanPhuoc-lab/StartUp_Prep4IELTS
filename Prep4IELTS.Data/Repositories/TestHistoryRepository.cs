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

    public async Task<IEnumerable<TestHistory>> FindAllByTestAndUserAsync(Guid testId, Guid userId)
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
                BandScore = th.BandScore,
                UserId = th.UserId,
                TestId = th.TestId
            }).ToListAsync();
    }

    public async Task<IEnumerable<TestHistory>> FindAllByUserIdAsync(Guid userId)
    {
        // Retrieve TestHistories with related PartitionHistories
        var testHistoryEntities =  await _dbSet
            .Where(th => th.UserId.Equals(userId))
            .Include(ph => ph.Test)
            .Include(ph => ph.PartitionHistories)
            .AsSplitQuery()
            .ToListAsync();

        // Filter the PartitionHistories to ensure distinct TestSectionPartId
        foreach (var th in testHistoryEntities)
        {
            if(th.Test.TestHistories.Any()) th.Test.TestHistories.Clear();
            th.PartitionHistories = th.PartitionHistories
                .GroupBy(ph => ph.TestSectionName)
                .Select(g => g.First())
                .ToList();
        }

        return testHistoryEntities;
    }

    public async Task<TestHistory?> FindByIdWithIncludePartitionAndGrade(int testHistoryId)
    {
        return await _dbSet.AsSplitQuery()
            .Where(x => x.TestHistoryId == testHistoryId)
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
                BandScore = th.BandScore,
                UserId = th.UserId,
                TestId = th.TestId,
                PartitionHistories = th.PartitionHistories.Select(ph => new PartitionHistory()
                {
                    PartitionHistoryId = ph.PartitionHistoryId,
                    TestSectionName = ph.TestSectionName,
                    TotalRightAnswer = ph.TotalRightAnswer,
                    TotalWrongAnswer = ph.TotalWrongAnswer,
                    TotalSkipAnswer = ph.TotalSkipAnswer,
                    AccuracyRate = ph.AccuracyRate,
                    TotalQuestion = ph.TotalQuestion,
                    TestHistoryId = ph.TestHistoryId,
                    TestSectionPartId = ph.TestSectionPartId,
                    TestSectionPart = new TestSectionPartition()
                    {
                        TestSectionPartId = ph.TestSectionPart.TestSectionPartId,
                        IsVerticalLayout = ph.TestSectionPart.IsVerticalLayout,
                        PartitionTagId = ph.TestSectionPart.PartitionTagId,
                        PartitionTag = new PartitionTag()
                        {
                            PartitionTagId = ph.TestSectionPart.PartitionTag.PartitionTagId,
                            PartitionTagDesc = ph.TestSectionPart.PartitionTag.PartitionTagDesc,
                        },
                    },
                    TestGrades = ph.TestGrades.Select(tg => new TestGrade()
                    {
                        TestGradeId = tg.TestGradeId,
                        GradeStatus = tg.GradeStatus,
                        QuestionNumber = tg.QuestionNumber,
                        RightAnswer = tg.RightAnswer,
                        InputedAnswer = tg.InputedAnswer,
                        QuestionId = tg.QuestionId,
                        PartitionHistoryId = tg.PartitionHistoryId,
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }
}