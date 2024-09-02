using System.Linq.Expressions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Business.Services;

public class TestService(
    UnitOfWork unitOfWork, 
    ITestHistoryService testHistoryService,
    ITestSectionService testSectionService,
    IQuestionService questionService,
    IScoreCalculationService scoreCalculationService) : ITestService
{
    // Basic
    public async Task<bool> InsertAsync(TestDto test)
    {
        await unitOfWork.TestRepository.InsertAsync(test.Adapt<Test>());
        return await unitOfWork.TestRepository.SaveChangeAsync() > 0;
    }

    public async Task<bool> InsertAsync(TestDto test, List<int>? tagIds)
    {
        await unitOfWork.TestRepository.InsertAsync(test.Adapt<Test>());
        var isCreateSuccess = await unitOfWork.TestRepository.SaveChangeAsync() > 0;

        if (isCreateSuccess)
        {
            // Get created test 
            var testEntity =
                await unitOfWork.TestRepository.FindOneWithConditionAsync(null, orderBy: tst => 
                    tst.OrderByDescending(x => x.Id));

            if (testEntity != null && 
                tagIds != null && tagIds.Any())
            {
                var isAddTagForTestSuccess = await unitOfWork.TestRepository.InsertTagsForTestAsync(testEntity.Id, tagIds);

                if (!isAddTagForTestSuccess) isCreateSuccess = false;
            }
        }

        return isCreateSuccess;
    }

    public async Task<bool> InsertReadingTestAsync(TestDto test)
    {
        throw new NotImplementedException();
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
        Expression<Func<Test, bool>>? filter, 
        Func<IQueryable<Test>, IOrderedQueryable<Test>>? orderBy = null, 
        string? includeProperties = "")
    {
        var testEntities = 
            await unitOfWork.TestRepository.FindOneWithConditionAsync(
                filter:filter, includeProperties: includeProperties);
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

    public async Task<bool> SubmitTestAsync(int totalCompletionTime, DateTime takenDate, bool isFull, Guid userId, int testId,
        List<QuestionAnswerSubmissionModel> questionAnswers)
    {
        // Get test by id 
        var testEntities = await unitOfWork.TestRepository.FindAllWithConditionAndThenIncludeAsync(
            // With conditions
            filter:x => x.Id == testId,
            orderBy: null,
            includes: [
                query => query.Include(tst => tst.TestSections)
                    // Then include list of section partitions
                    .ThenInclude(ts => ts.TestSectionPartitions)
                        // Then include list of questions
                        .ThenInclude(tsp => tsp.Questions)
            ]);
        // Check exist test 
        if (!testEntities.Any()) return false;

        // First test found
        var singleTestEntity = testEntities.First();
        // Get test section partitions 
        var sectionPartitions = singleTestEntity.TestSections.SelectMany(ts => 
            ts.TestSectionPartitions).ToList();
        // Partition history collection initiation  
        List<PartitionHistory> partitionHistories = new();
        // Iterate each section partitions, create partition history
        foreach (var tsp in sectionPartitions)
        {
            // Get section include this partition
            var sectionHoldPartition = singleTestEntity.TestSections.First(ts =>
                ts.TestSectionPartitions.Any(tstSectPart => tstSectPart.TestSectionPartId == tsp.TestSectionPartId));
            
            // Initiate partition history
            PartitionHistory partitionHistory = new ();
            
            // Get question answers, which have the same id with list of question is partitions
            var questionAnswersInPartition =
                questionAnswers.Where(x => 
                    tsp.Questions.Any(q => q.QuestionId == x.QuestionId))
                .ToList();
            
            // Iterate each question answer, create test grade dto 
            foreach (var qa in questionAnswersInPartition)
            {
                // Get question by id 
                var questionDto = await questionService.FindQuestionByIdAndWithQuestions(qa.QuestionId);
                // Check correctness of the selected answer with list of answers 
                var isSelectedAnswerCorrect =
                    questionDto.QuestionAnswers.Any(x => 
                        x.AnswerText.ToUpper().Equals(qa.SelectedAnswer.ToUpper()) && x.IsTrue);
                // Create grade status whether the selected answer is correct or not 
                var gradeStatusForRightWrong = isSelectedAnswerCorrect
                    ? GradeStatus.Correct.GetDescription()
                    : GradeStatus.Wrong.GetDescription();   
                // Create right answer text
                var rightAnswer = questionDto.QuestionAnswers
                    .Where(ans => ans.IsTrue)
                    .Select(ans => ans.AnswerDisplay)
                    .First();
                // Add new test grade
                partitionHistory.TestGrades.Add(new TestGrade()
                {
                    QuestionId = qa.QuestionId,
                    InputedAnswer = qa.SelectedAnswer,
                    // Check if selected answer is not empty
                    GradeStatus = !string.IsNullOrEmpty(qa.SelectedAnswer) 
                        // Right/Wrong
                        ? gradeStatusForRightWrong
                        // Skip
                        : GradeStatus.Skip.GetDescription(),
                    QuestionNumber = questionDto.QuestionNumber,
                    RightAnswer = rightAnswer
                });
            }
            
            // Partition history's test grades
            var pHistoryTestGrades = partitionHistory.TestGrades;
            // Count total right, wrong, skip, accuracy rate
            partitionHistory.TotalRightAnswer = 
                pHistoryTestGrades.Count(x => x.GradeStatus.Equals(GradeStatus.Correct.GetDescription()));
            partitionHistory.TotalWrongAnswer = 
                pHistoryTestGrades.Count(x => x.GradeStatus.Equals(GradeStatus.Wrong.GetDescription()));
            partitionHistory.TotalSkipAnswer = 
                pHistoryTestGrades.Count(x => x.GradeStatus.Equals(GradeStatus.Skip.GetDescription()));
            
            // Other properties
            // Test section name
            partitionHistory.TestSectionName = sectionHoldPartition.TestSectionName;
            // Total question
            partitionHistory.TotalQuestion = tsp.Questions.Count;
            partitionHistory.TestSectionPartId = tsp.TestSectionPartId;
            // Accuracy rate
            partitionHistory.AccuracyRate = 
                partitionHistory.TotalRightAnswer / (double)partitionHistory.TotalQuestion;
            
            // Add to list of partition history 
            partitionHistories.Add(partitionHistory);
        }
        
        // Initiate test history 
        TestHistory testHistory = new()
        {
            UserId = userId,
            TestId = singleTestEntity.TestId,
            IsFull = isFull,
            TakenDate = takenDate,
            TestType = singleTestEntity.TestType,
            TestCategoryId = singleTestEntity.TestCategoryId,
            TotalCompletionTime = totalCompletionTime,
            PartitionHistories = partitionHistories,
            TotalQuestion = singleTestEntity.TotalQuestion
        };
        
        // Count total right, wrong, skip, accuracy rate for all question in history
        testHistory.TotalRightAnswer = partitionHistories.Sum(ph => ph.TotalRightAnswer);
        testHistory.TotalWrongAnswer = partitionHistories.Sum(ph => ph.TotalWrongAnswer);
        testHistory.TotalSkipAnswer = partitionHistories.Sum(ph => ph.TotalSkipAnswer);
        // Accuracy rate
        testHistory.AccuracyRate = 
            testHistory.TotalRightAnswer / (double)testHistory.TotalQuestion;
        
        // Get score calculation
        var scoreCalculationDto =
            await scoreCalculationService.GetByTotalRightAnswerAndTestType(testHistory.TotalRightAnswer!.Value,
                testHistory.TestType);
        // Update history band score 
        if (scoreCalculationDto != null!)
        {
            testHistory.BandScore = scoreCalculationDto.BandScore;
            testHistory.ScoreCalculationId = scoreCalculationDto.ScoreCalculationId;
        }
        else
        {
            testHistory.BandScore = "0";
        }
        
        // Update total engaged in test 
        singleTestEntity.TotalEngaged += 1;
        await unitOfWork.TestRepository.UpdateAsync(singleTestEntity);
        
        // Add history to DB
        return await testHistoryService.InsertAsync(testHistory.Adapt<TestHistoryDto>());
    }

    public async Task<int> CountTotalAsync()
    {
        return await unitOfWork.TestRepository.CountTotalAsync();
    }

    public async Task<bool> IsExistTestAsync(int id)
    {
        return await unitOfWork.TestRepository.IsExistTestAsync(id);
    }
}