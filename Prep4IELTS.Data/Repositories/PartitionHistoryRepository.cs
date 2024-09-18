using System.Runtime.Remoting;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class PartitionHistoryRepository : GenericRepository<PartitionHistory>
{
    public PartitionHistoryRepository(Prep4IeltsContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<PartitionHistory?> FindByIdAndGradeAsync(int id, int testGradeId, bool? hasPremiumPackage)
    {
        return await _dbSet.Where(ph => ph.PartitionHistoryId == id)
            .AsSplitQuery()
            .Select(ph => new PartitionHistory()
            {
                PartitionHistoryId = ph.PartitionHistoryId,
                TestSectionName = ph.TestSectionName,
                TestHistoryId = ph.TestHistoryId,
                TestSectionPartId = ph.TestSectionPartId,
                TestSectionPart = new TestSectionPartition()
                {
                    TestSectionPartId = ph.TestSectionPart.TestSectionPartId,
                    PartitionDesc = ph.TestSectionPart.PartitionDesc,
                    CloudResource = ph.TestSectionPart.CloudResource != null!
                        ? new CloudResource()
                        {
                            CloudResourceId = ph.TestSectionPart.CloudResource.CloudResourceId,
                            PublicId = ph.TestSectionPart.CloudResource.PublicId,
                            Url = ph.TestSectionPart.CloudResource.Url,
                            Bytes = ph.TestSectionPart.CloudResource.Bytes,
                            CreateDate = ph.TestSectionPart.CloudResource.CreateDate,
                            ModifiedDate = ph.TestSectionPart.CloudResource.ModifiedDate
                        }
                        : null!,
                    TestSectionId = ph.TestSectionPart.TestSectionId,
                    TestSection = new TestSection()
                    {
                        TestSectionId = ph.TestSectionPart.TestSection.TestSectionId,
                        // TestSectionName = ph.TestSectionPart.TestSection.TestSectionName,
                        ReadingDesc = ph.TestSectionPart.TestSection.ReadingDesc,
                        // AudioResourceUrl = ph.TestSectionPart.TestSection.AudioResourceUrl,
                        CloudResource = ph.TestSectionPart.TestSection.CloudResource != null!
                            ? new CloudResource()
                            {
                                CloudResourceId = ph.TestSectionPart.TestSection.CloudResource.CloudResourceId,
                                PublicId = ph.TestSectionPart.TestSection.CloudResource.PublicId,
                                Url = ph.TestSectionPart.TestSection.CloudResource.Url,
                                Bytes = ph.TestSectionPart.TestSection.CloudResource.Bytes,
                                CreateDate = ph.TestSectionPart.TestSection.CloudResource.CreateDate,
                                ModifiedDate = ph.TestSectionPart.TestSection.CloudResource.ModifiedDate
                            }
                            : null!,
                        SectionTranscript = ph.TestSectionPart.TestSection.SectionTranscript,
                        // Test = new Test()
                        // {
                        //     TestId = ph.TestSectionPart.TestSection.Test.TestId,
                        //     TestTitle = ph.TestSectionPart.TestSection.Test.TestTitle,
                        // }
                    },
                    PartitionTagId = ph.TestSectionPart.PartitionTagId,
                    PartitionTag = ph.TestSectionPart.PartitionTag
                },
                TestGrades = ph.TestGrades
                    .Where(tg => tg.TestGradeId == testGradeId)
                    .Select(tg => new TestGrade()
                    {
                        TestGradeId = tg.TestGradeId,
                        GradeStatus = tg.GradeStatus,
                        QuestionNumber = tg.QuestionNumber,
                        RightAnswer = tg.RightAnswer,
                        InputedAnswer = tg.InputedAnswer,
                        QuestionId = tg.QuestionId,
                        PartitionHistoryId = tg.PartitionHistoryId,
                        Question = new Question()
                        {
                            QuestionId = tg.Question.QuestionId,
                            QuestionDesc = tg.Question.QuestionDesc,
                            QuestionNumber = tg.Question.QuestionNumber,
                            IsMultipleChoice = tg.Question.IsMultipleChoice,
                            TestSectionPartId = tg.Question.TestSectionPartId,
                            QuestionAnswers = tg.Question.QuestionAnswers,
                            QuestionAnswerExplanation = hasPremiumPackage.HasValue && hasPremiumPackage.Value
                                ? tg.Question.QuestionAnswerExplanation : null
                        }
                    })
                    .ToList()
            }).FirstOrDefaultAsync();
    }
    
}