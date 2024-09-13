using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Enum;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data.Repositories;

public class TestGradeRepository : GenericRepository<TestGrade>
{
    public TestGradeRepository(Prep4IeltsContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<List<TestGrade>> FindAllTestGradeByHistoryId(int testHistoryId)
    {
        return await DbContext.PartitionHistories
            .Where(ph => ph.TestHistoryId == testHistoryId)
            .Include(ph => ph.TestGrades)
            .SelectMany(ph => ph.TestGrades)
            .ToListAsync();
    }


    public async Task<bool> UpdateTestGradesAsync(List<TestGrade> testGrades, int testHistoryId, int totalCompletionTime, DateTime takenDate)
    {
        // Update test and partition history report
        var testHistory = DbContext.TestHistories
            .Include(th => th.PartitionHistories)
            .ThenInclude(tph => tph.TestGrades)
            .FirstOrDefault(t => t.TestHistoryId == testHistoryId);

        // Throw exception whenever test grade not exist 
        if (testHistory == null)
            throw new NullReferenceException(
                $"Not found any test history match id {testHistoryId}");

        foreach (var ph in testHistory.PartitionHistories.ToList())
        {
            var questionIdsInPartition = ph.TestGrades.Select(tg => tg.QuestionId).ToList();
            var testGradesToUpdate = testGrades
                    .Where(tg => questionIdsInPartition.Any(qId => tg.QuestionId == qId))
                .ToList();
            
            // Set default partition report
            ph.TotalRightAnswer = 0;
            ph.TotalWrongAnswer = 0;
            ph.TotalSkipAnswer = 0;
            
            foreach (var tg in testGradesToUpdate)
            {
                // Get test grade by id
                var testGradeEntity = await
                    _dbSet.Where(tgE =>
                            tgE.PartitionHistoryId == ph.PartitionHistoryId && tgE.QuestionId == tg.QuestionId)
                        .FirstOrDefaultAsync();

                // Throw exception whenever test grade not exist 
                if (testGradeEntity == null)
                    throw new NullReferenceException(
                        $"Not found any test grade has question id {tg.QuestionId}");

                // Get question by id
                var question = await DbContext.Questions
                    .Where(q => q.QuestionId == tg.QuestionId)
                    .FirstOrDefaultAsync();

                // Throw exception whenever question not exist 
                if (question == null)
                    throw new NullReferenceException(
                        $"Not found any question match id {tg.QuestionId}");

                // Check whether multiple choice question
                var isMultipleChoice = question.IsMultipleChoice;

                // Retrieve list answers by question id
                var questionAnswers =
                    await DbContext.QuestionAnswers.Where(qa => qa.QuestionId == tg.QuestionId).ToListAsync();

                // Check correctness
                var isCorrect = !isMultipleChoice // Is not multiple choice
                    // Compare with answer text
                    ? questionAnswers.Any(qa => qa.AnswerText.Equals(tg.InputedAnswer))
                    // Compare with answer display (A, B, C, D, ...)
                    : questionAnswers.Any(qa => qa.AnswerDisplay.Equals(tg.InputedAnswer));

                // Check empty answer
                if (string.IsNullOrEmpty(tg.InputedAnswer))
                {
                    testGradeEntity.GradeStatus = GradeStatus.Skip.GetDescription();
                    // Increase skip answer to 1
                    ph.TotalSkipAnswer += 1;
                }
                else
                {
                    // Update grade status
                    testGradeEntity.GradeStatus = isCorrect
                        ? GradeStatus.Correct.GetDescription()
                        : GradeStatus.Wrong.GetDescription();
                    
                    // Increase skip answer to 1
                    if(isCorrect) ph.TotalRightAnswer += 1;
                    else ph.TotalWrongAnswer += 1;
                }

                // Update input text
                testGradeEntity.InputedAnswer = tg.InputedAnswer;
            }
            
            ph.AccuracyRate = (ph.TotalRightAnswer < 1)
                ? 0
                // Total right answer / total question in partition
                : ph.TotalRightAnswer / questionIdsInPartition.Count;
        }
        
        // Update history report
        // Count total right, wrong, skip, accuracy rate for all question in history
        testHistory.TotalRightAnswer = testHistory.PartitionHistories.Sum(ph => ph.TotalRightAnswer);
        testHistory.TotalWrongAnswer = testHistory.PartitionHistories.Sum(ph => ph.TotalWrongAnswer);
        testHistory.TotalSkipAnswer = testHistory.PartitionHistories.Sum(ph => ph.TotalSkipAnswer);
        // Accuracy rate
        testHistory.AccuracyRate = (testHistory.TotalRightAnswer < 1)
            ? 0
            : testHistory.TotalRightAnswer / (double)testHistory.TotalQuestion;

        // Get score calculation
        var totalRightAnswer = testHistory.TotalRightAnswer!.Value;
        var scoreCalculation = 
            await DbContext.ScoreCalculations.FirstOrDefaultAsync(sc =>
                sc.FromTotalRight <= totalRightAnswer && sc.ToTotalRight >= totalRightAnswer &&
                sc.TestType.Equals(testHistory.TestType));

        // Update history band score 
        if (scoreCalculation != null!)
        {
            testHistory.BandScore = scoreCalculation.BandScore;
            testHistory.ScoreCalculationId = scoreCalculation.ScoreCalculationId;
        }
        else
        {
            testHistory.BandScore = "0";
        }
        
        // Update total completion time
        testHistory.TotalCompletionTime = totalCompletionTime;
        // Update taken date
        testHistory.TakenDate = takenDate;

        // Save re-submit test grades
        return await SaveChangeWithTransactionAsync() > 0;
    }
}