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

    public async Task<bool> InsertTagsForTestAsync(int id, List<int> tagIds)
    {
        var testEntity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        if (testEntity == null) return false;

        foreach (var tagId in tagIds)
        {
            var tagEntity = await DbContext.Tags.FirstOrDefaultAsync(tg => tg.TagId == tagId);
            if (tagEntity != null)
            {
                testEntity.Tags.Add(tagEntity);
            }
        }

        return await SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> UpdateTagsForTestAsync(int id, List<int> tagIds)
    {
        var testEntity = await _dbSet.Include(tst => 
            tst.Tags).FirstOrDefaultAsync(x => x.Id == id);
        if (testEntity == null) return false;
        
        // Clear existing tags
        testEntity.Tags.Clear();

        foreach (var tagId in tagIds)
        {
            var tagEntity = await DbContext.Tags.FirstOrDefaultAsync(tg => tg.TagId == tagId);
            if (tagEntity != null)
            {
                testEntity.Tags.Add(tagEntity);
            }
        }

        return await SaveChangeWithTransactionAsync() > 0;
    }

    public override async Task RemoveAsync(object id)
    {
        var isGuid = Guid.TryParse(id.ToString(), out var idGuid);
        int.TryParse(id.ToString(), out var idInt);
        
        var testEntity = await _dbSet
            .Include(tst => tst.Tags) 
            .Include(tst => tst.TestHistories)
                .ThenInclude(th => th.PartitionHistories)
                    .ThenInclude(ph => ph.TestGrades)
            .Include(tst => tst.TestSections)
                .ThenInclude(ts => ts.TestSectionPartitions)
                    .ThenInclude(tsp => tsp.Questions)
                        .ThenInclude(qs => qs.QuestionAnswers)
            .Where(tst => isGuid ? tst.TestId == idGuid : tst.Id == idInt)
            .FirstOrDefaultAsync();
        
        if (testEntity == null) throw new NullReferenceException($"Not found any test match id {id}");
        
        // Remove tags from testEntity.Tags if they exist
        foreach (var tag in testEntity.Tags.ToList())
        {
            var tagEntity = await DbContext.Tags
                .FirstOrDefaultAsync(x => x.TagId == tag.TagId);
            if (tagEntity != null)
            {
                testEntity.Tags.Remove(tagEntity);
            }
        }
        await SaveChangeAsync();
        
        // Remove all test history
        DbContext.TestHistories.RemoveRange(testEntity.TestHistories);
        
        // Remove test 
        _dbSet.Remove(testEntity);
    }

    public async Task<bool> RemoveAllCloudResourceAsync(Guid testId)
    {
        var test = await _dbSet
            .Include(x => x.TestSections)
            .ThenInclude(x => x.CloudResource)
            .FirstOrDefaultAsync(tst => tst.TestId == testId);
        
        var testSectionIds = test!.TestSections.Select(ts => ts.TestSectionId).ToList();
        var partitionCloudResources = await DbContext.TestSectionPartitions
            .Include(tsp => tsp.CloudResource)
            .Where(tsp => testSectionIds.Contains(tsp.TestSectionId) && tsp.CloudResource != null!)
            .Select(tsp => tsp.CloudResource)
            .ToListAsync();
        
        // Add cloud sources of test sections
        partitionCloudResources.AddRange(test.TestSections
            .Select(tsp => tsp.CloudResource));

        // Remove range
        if(partitionCloudResources.Any())
            DbContext.CloudResources.RemoveRange(partitionCloudResources.Where(cr => cr != null)!);
        // Save change
        return await SaveChangeWithTransactionAsync() > 0;
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

                // Add AsSplitQuery when includes are present
                query = query.AsSplitQuery();
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        else
        {
            query = query.OrderBy(x => x.Id);
        }

        // Check whether pageIndex < 1
        if (!pageIndex.HasValue || pageIndex.Value < 1) pageIndex = 1;
        // Check whether pageSize < 1
        if (!pageSize.HasValue || pageSize.Value < 1) pageSize = 10;

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

    public async Task<Test?> FindByIdForPracticeAsync(int id, int[] sectionIds)
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
                        // AudioResourceUrl = ts.AudioResourceUrl,
                        CloudResource = ts.CloudResource != null!
                            ? new CloudResource()
                            {
                                CloudResourceId = ts.CloudResource.CloudResourceId,
                                PublicId = ts.CloudResource.PublicId,
                                Url = ts.CloudResource.Url,
                                Bytes = ts.CloudResource.Bytes,
                                CreateDate = ts.CloudResource.CreateDate,
                                ModifiedDate = ts.CloudResource.ModifiedDate
                            }
                            : null!,
                        TotalQuestion = ts.TotalQuestion,
                        TestSectionPartitions = ts.TestSectionPartitions.Select(tsp =>
                            new TestSectionPartition()
                            {
                                TestSectionPartId = tsp.TestSectionPartId,
                                PartitionDesc = tsp.PartitionDesc,
                                IsVerticalLayout = tsp.IsVerticalLayout,
                                CloudResource = tsp.CloudResource != null!
                                    ? new CloudResource()
                                    {
                                        CloudResourceId = tsp.CloudResource.CloudResourceId,
                                        PublicId = tsp.CloudResource.PublicId,
                                        Url = tsp.CloudResource.Url,
                                        Bytes = tsp.CloudResource.Bytes,
                                        CreateDate = tsp.CloudResource.CreateDate,
                                        ModifiedDate = tsp.CloudResource.ModifiedDate
                                    }
                                    : null!,
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
            }).FirstOrDefaultAsync();
    }

    public async Task<Test?> FindByIdForTestSimulationAsync(int id)
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
                        // AudioResourceUrl = ts.AudioResourceUrl,
                        CloudResource = ts.CloudResource != null!
                            ? new CloudResource()
                            {
                                CloudResourceId = ts.CloudResource.CloudResourceId,
                                PublicId = ts.CloudResource.PublicId,
                                Url = ts.CloudResource.Url,
                                Bytes = ts.CloudResource.Bytes,
                                CreateDate = ts.CloudResource.CreateDate,
                                ModifiedDate = ts.CloudResource.ModifiedDate
                            }
                            : null!,
                        TotalQuestion = ts.TotalQuestion,
                        TestSectionPartitions = ts.TestSectionPartitions.Select(tsp =>
                            new TestSectionPartition()
                            {
                                TestSectionPartId = tsp.TestSectionPartId,
                                PartitionDesc = tsp.PartitionDesc,
                                IsVerticalLayout = tsp.IsVerticalLayout,
                                // PartitionImage = tsp.PartitionImage,
                                CloudResource = tsp.CloudResource != null!
                                    ? new CloudResource()
                                    {
                                        CloudResourceId = tsp.CloudResource.CloudResourceId,
                                        PublicId = tsp.CloudResource.PublicId,
                                        Url = tsp.CloudResource.Url,
                                        Bytes = tsp.CloudResource.Bytes,
                                        CreateDate = tsp.CloudResource.CreateDate,
                                        ModifiedDate = tsp.CloudResource.ModifiedDate
                                    }
                                    : null!,
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
            }).FirstOrDefaultAsync();
    }

    public async Task<Test?> FindByIdAndGetAllAnswerAsync(int id)
    {
        var test = await _dbSet.Where(x => x.Id == id)
            .AsSplitQuery()
            .Select(tst => new Test()
            {
                Id = tst.Id,
                TestId = tst.TestId,
                TestTitle = tst.TestTitle,
                TestCategoryId = tst.TestCategoryId,
                TestSections = tst.TestSections
                    .Select(ts => new TestSection()
                    {
                        TestSectionId = ts.TestSectionId,
                        TestSectionName = ts.TestSectionName,
                        // SectionTranscript = ts.SectionTranscript,
                        TestSectionPartitions = ts.TestSectionPartitions.Select(tsp =>
                            new TestSectionPartition()
                            {
                                TestSectionPartId = tsp.TestSectionPartId,
                                TestSectionId = tsp.TestSectionId,
                                PartitionTagId = tsp.PartitionTagId,
                                Questions = tsp.Questions.Select(qs => new Question()
                                {
                                    QuestionId = qs.QuestionId,
                                    QuestionDesc = qs.QuestionDesc,
                                    QuestionNumber = qs.QuestionNumber,
                                    IsMultipleChoice = qs.IsMultipleChoice,
                                    TestSectionPartId = qs.TestSectionPartId,
                                    QuestionAnswers = qs.QuestionAnswers
                                        .Where(qAns => qAns.IsTrue)
                                        .Select(qAns => new QuestionAnswer()
                                        {
                                            QuestionAnswerId = qAns.QuestionAnswerId,
                                            AnswerText = qAns.AnswerText,
                                            AnswerDisplay = qAns.AnswerDisplay,
                                            IsTrue = qAns.IsTrue,
                                            QuestionId = qAns.QuestionId
                                        }).ToList()
                                }).ToList()
                            }).ToList()
                    }).ToList()
            }).FirstOrDefaultAsync();

        if (test == null) return null!;
        var testSectionParts = test.TestSections
            .SelectMany(x => x.TestSectionPartitions)
            .ToList();

        foreach (var tsp in testSectionParts)
        {
            foreach (var q in tsp.Questions)
            {
                q.QuestionAnswers = q.QuestionAnswers
                    .GroupBy(qa => qa.AnswerDisplay)
                    .Select(g => g.First())
                    .ToList();
            }
        }

        return test;
    }

    public async Task<List<Comment>> FindAllCommentByTestIdAsync(Guid id)
    {
        return await DbContext.Comments.Where(cmt => cmt.TestId == id).ToListAsync();
    }
    
    public async Task<string> FindTestSectionTranscriptAsync(int testSectionId)
    {
        var testSection = await DbContext.TestSections.FirstOrDefaultAsync(x =>
            x.TestSectionId == testSectionId);
        if (testSection == null) return string.Empty;
        return testSection.SectionTranscript ?? string.Empty;
    }

    public async Task<int> CountTotalAsync()
    {
        return await _dbSet.CountAsync();
    }
    
    public async Task<int> CountTotalDraftAsync()
    {
        return await _dbSet.Where(t => t.IsDraft).CountAsync();
    }
    
    public async Task<int> CountTotalPublishAsync()
    {
        return await _dbSet.Where(t => !t.IsDraft).CountAsync();
    }

    public async Task<bool> IsExistTestAsync(int id)
    {
        return await _dbSet.AnyAsync(x => x.Id == id);
    }
    
    public async Task<bool> IsExistTestAsync(Guid id)
    {
        return await _dbSet.AnyAsync(x => x.TestId == id);
    }

    public async Task<bool> IsPublishedAsync(Guid id)
    {
        return await _dbSet.AnyAsync(x => x.TestId == id && !x.IsDraft);
    }

    public async Task<bool> PublishTestAsync(Guid id)
    {
        var testEntity = await _dbSet.FirstOrDefaultAsync(x => x.TestId == id);
        
        if(testEntity == null) throw new NullReferenceException($"Not found any test match id {id}");
        // Change test status to publish
        testEntity.IsDraft = false;

        return await SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> HideTestAsync(Guid id)
    {
        var testEntity = await _dbSet.FirstOrDefaultAsync(x => x.TestId == id);
        
        if(testEntity == null) throw new NullReferenceException($"Not found any test match id {id}");
        // Change test status to publish
        testEntity.IsDraft = true;

        return await SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<bool> IsExistAnyHistoryAsync(Guid id)
    {
        return await _dbSet.Where(x => x.TestId == id).AnyAsync(x => x.TestHistories.Any());
    }
}