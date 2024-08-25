using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class TestRepository : GenericRepository<Test>
{
    public TestRepository(Prep4IeltsContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<IEnumerable<Test>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Test, bool>>? filter,
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy,
        string? includeProperties, int? pageIndex, int? pageSize)
    {
        IQueryable<Test> query = _dbSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(
                         new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        // Check whether pageIndex < 1
        if (!pageIndex.HasValue || pageIndex.Value < 1) pageIndex = 1;
        // Check whether pageSize < 1
        if (!pageSize.HasValue || pageIndex.Value < 1) pageSize = 10;

        // Count offset
        var skipOffset = (pageIndex.Value - 1) * pageSize.Value;

        var result = await query
            // Skip elements
            .Skip(skipOffset)
            // Take total page elements
            .Take(pageSize.Value)
            // Convert to List
            .ToListAsync();

        // If result is empty and pageIndex > 1, reset to first page
        if (!result.Any() && pageIndex > 1)
        {
            result = await query.Take(pageSize.Value).ToListAsync();
        }

        return result;
    }
    public async Task<IList<Test>> FindByIdForPracticeAsync(int id, int[] sectionIds)
    {
        // Combine 'Include' and 'Select' can be problematic
        // 'Include' primarily used to include related data from other tables, but using it in combination
        // with 'Select' can lead to unintended behaviour because 'Include' is meant for eager loading, not projection.
        // return await _dbSet.Where(tst => tst.Id == id)
        //     .Include(tst => tst.TestSections.Where(ts =>
        //             sectionIds.Contains(ts.TestSectionId))
        //         .Select(ts => new TestSection()
        //         {
        //             TestSectionId = ts.TestSectionId,
        //             TestSectionName = ts.TestSectionName,
        //             ReadingDesc = ts.ReadingDesc,
        //             AudioResourceUrl = ts.AudioResourceUrl,
        //             TotalQuestion = ts.TotalQuestion,
        //             TestSectionPartitions = ts.TestSectionPartitions.Select(tsp =>
        //                 new TestSectionPartition()
        //             {
        //                 TestSectionPartId = tsp.TestSectionPartId,
        //                 PartitionDesc = tsp.PartitionDesc,
        //                 IsVerticalLayout = tsp.IsVerticalLayout,
        //                 PartitionImage = tsp.PartitionImage,
        //                 TestSectionId = tsp.TestSectionId,
        //                 PartitionTag = tsp.PartitionTag,
        //                 Questions = tsp.Questions.Select(qs => new Question()
        //                 {
        //                     QuestionId = qs.QuestionId,
        //                     QuestionDesc = qs.QuestionDesc,
        //                     QuestionNumber = qs.QuestionNumber,
        //                     IsMultipleChoice = qs.IsMultipleChoice,
        //                     TestSectionPartId = qs.TestSectionPartId,
        //                     QuestionAnswers = qs.IsMultipleChoice 
        //                         ? qs.QuestionAnswers.Select(qAns => new QuestionAnswer()
        //                         {
        //                             QuestionAnswerId = qAns.QuestionAnswerId,
        //                             AnswerText = qAns.AnswerText,
        //                             IsTrue = qAns.IsTrue,
        //                             QuestionId = qAns.QuestionId
        //                         }).ToList()
        //                         : null!
        //                 }).ToList()
        //             }).ToList()
        //         })).ToListAsync();

        return await _dbSet
            .Where(tst => tst.Id == id)
            .Select(tst => new Test()
            {
                Id = tst.Id,
                TestId = tst.TestId,
                TestTitle = tst.TestTitle,
                Duration = tst.Duration,
                TestType = tst.TestType,
                TotalEngaged = tst.TotalEngaged,
                TotalQuestion = tst.TotalQuestion,
                TotalSection = tst.TotalSection,
                TestCategoryId = tst.TestCategoryId,
                TestSections = tst.TestSections
                    .Where(ts => sectionIds.Contains(ts.TestSectionId))
                    .Select(ts => new TestSection()
                    {
                        TestSectionId = ts.TestSectionId,
                        TestSectionName = ts.TestSectionName,
                        ReadingDesc = ts.ReadingDesc,
                        AudioResourceUrl = ts.AudioResourceUrl,
                        TotalQuestion = ts.TotalQuestion,
                        TestSectionPartitions = ts.TestSectionPartitions.Select(tsp =>
                            new TestSectionPartition()
                            {
                                TestSectionPartId = tsp.TestSectionPartId,
                                PartitionDesc = tsp.PartitionDesc,
                                IsVerticalLayout = tsp.IsVerticalLayout,
                                PartitionImage = tsp.PartitionImage,
                                TestSectionId = tsp.TestSectionId,
                                PartitionTagId = tsp.PartitionTagId,
                                Questions = tsp.Questions.Select(qs => new Question()
                                {
                                    QuestionId = qs.QuestionId,
                                    QuestionDesc = qs.QuestionDesc,
                                    QuestionNumber = qs.QuestionNumber,
                                    IsMultipleChoice = qs.IsMultipleChoice,
                                    TestSectionPartId = qs.TestSectionPartId,
                                    QuestionAnswers = qs.IsMultipleChoice
                                        ? qs.QuestionAnswers.Select(qAns => new QuestionAnswer()
                                        {
                                            QuestionAnswerId = qAns.QuestionAnswerId,
                                            AnswerText = qAns.AnswerText,
                                            IsTrue = qAns.IsTrue,
                                            QuestionId = qAns.QuestionId
                                        }).ToList()
                                        : null!
                                }).ToList()
                            }).ToList()
                    }).ToList()
            }).ToListAsync();
    }

    public async Task<IList<Test>> FindByIdForTestSimulationAsync(int id)
    {
        return await _dbSet
            .Where(tst => tst.Id == id)
            .Select(tst => new Test()
            {
                Id = tst.Id,
                TestId = tst.TestId,
                TestTitle = tst.TestTitle,
                Duration = tst.Duration,
                TestType = tst.TestType,
                TotalEngaged = tst.TotalEngaged,
                TotalQuestion = tst.TotalQuestion,
                TotalSection = tst.TotalSection,
                TestCategoryId = tst.TestCategoryId,
                TestSections = tst.TestSections
                    .Select(ts => new TestSection()
                    {
                        TestSectionId = ts.TestSectionId,
                        TestSectionName = ts.TestSectionName,
                        ReadingDesc = ts.ReadingDesc,
                        AudioResourceUrl = ts.AudioResourceUrl,
                        TotalQuestion = ts.TotalQuestion,
                        TestSectionPartitions = ts.TestSectionPartitions.Select(tsp =>
                            new TestSectionPartition()
                            {
                                TestSectionPartId = tsp.TestSectionPartId,
                                PartitionDesc = tsp.PartitionDesc,
                                IsVerticalLayout = tsp.IsVerticalLayout,
                                PartitionImage = tsp.PartitionImage,
                                TestSectionId = tsp.TestSectionId,
                                PartitionTagId = tsp.PartitionTagId,
                                Questions = tsp.Questions.Select(qs => new Question()
                                {
                                    QuestionId = qs.QuestionId,
                                    QuestionDesc = qs.QuestionDesc,
                                    QuestionNumber = qs.QuestionNumber,
                                    IsMultipleChoice = qs.IsMultipleChoice,
                                    TestSectionPartId = qs.TestSectionPartId,
                                    QuestionAnswers = qs.IsMultipleChoice
                                        ? qs.QuestionAnswers.Select(qAns => new QuestionAnswer()
                                        {
                                            QuestionAnswerId = qAns.QuestionAnswerId,
                                            AnswerText = qAns.AnswerText,
                                            IsTrue = qAns.IsTrue,
                                            QuestionId = qAns.QuestionId
                                        }).ToList()
                                        : null!
                                }).ToList()
                            }).ToList()
                    }).ToList()
            }).ToListAsync();
    }
    
    public async Task<int> CountTotalAsync()
    {
        return await _dbSet.CountAsync();
    }
}