using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
    Task TrySeedAsync();
    Task SeedAsync();
}

public class DatabaseInitializer(Prep4IeltsContext dbContext) : IDatabaseInitializer
{
    //  Summary:
    //      Initialize Database
    public async Task InitializeAsync()
    {
        try
        {
            // Check whether database exist
            if (!await dbContext.Database.CanConnectAsync())
            {
                // Perform migration database
                await dbContext.Database.MigrateAsync();
            }

            // Check for applied migrations
            var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
            if (appliedMigrations.Any())
            {
                Console.WriteLine("Migrations have been applied.");
                return;
            }

            Console.WriteLine("Database initialized successfully");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //  Summary:
    //      Try to perform seeding data
    public async Task TrySeedAsync()
    {
        try
        {
            await SeedAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //  Summary:
    //      Seeding data
    public async Task SeedAsync()
    {
        try
        {
            // System roles
            if (!dbContext.SystemRoles.Any()) await SeedSystemRoleAsync();
            // Users
            if (!dbContext.Users.Any()) await SeedUserAsync();
            // Tags
            if (!dbContext.Tags.Any()) await SeedTagAsync();
            // Test categories
            if (!dbContext.TestCategories.Any()) await SeedTestCategoryAsync();
            // Tests
            if (!dbContext.Tests.Any()) await SeedTestAsync();
            // Tests History
            if (!dbContext.TestHistories.Any()) await SeedTestHistoryAsync();

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    //  Summary:
    //      Seeding system roles
    private async Task SeedSystemRoleAsync()
    {
        List<SystemRole> roles = new()
        {
            new() { RoleName = Enum.SystemRole.Admin.GetDescription() },
            new() { RoleName = Enum.SystemRole.Staff.GetDescription() },
            new() { RoleName = Enum.SystemRole.Student.GetDescription() }
        };

        await dbContext.SystemRoles.AddRangeAsync(roles);
        await dbContext.SaveChangesAsync();
    }

    //  Summary:
    //      Seeding user
    private async Task SeedUserAsync()
    {
        List<User> users = new()
        {
            new User
            {
                ClerkId = Guid.NewGuid().ToString(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(1990, 5, 15),
                Phone = "0978112391",
                IsActive = true,
                CreateDate = DateTime.Now,
                TestTakenDate = DateTime.Now.AddDays(-30),
                TargetScore = "8.0"
            },
            new User
            {
                ClerkId = Guid.NewGuid().ToString(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                DateOfBirth = new DateTime(1985, 10, 20),
                Phone = "0728532391",
                IsActive = false,
                CreateDate = DateTime.Now.AddMonths(-3),
                TestTakenDate = DateTime.Now.AddDays(-60),
                TargetScore = "6.0"
            },
            new User
            {
                ClerkId = Guid.NewGuid().ToString(),
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                DateOfBirth = new DateTime(1995, 7, 10),
                Phone = "07768935341",
                IsActive = true,
                CreateDate = DateTime.Now.AddMonths(-6),
                TestTakenDate = DateTime.Now.AddDays(-15),
                TargetScore = "5.5"
            },
            new User
            {
                ClerkId = Guid.NewGuid().ToString(),
                FirstName = "Bob",
                LastName = "Brown",
                Email = "bob.brown@example.com",
                DateOfBirth = new DateTime(1988, 12, 25),
                Phone = "0918685770",
                IsActive = true,
                CreateDate = DateTime.Now.AddMonths(-12),
                TestTakenDate = DateTime.Now.AddDays(-90),
                TargetScore = "6.5"
            },
            new User
            {
                ClerkId = Guid.NewGuid().ToString(),
                FirstName = "Eve",
                LastName = "Davis",
                Email = "eve.davis@example.com",
                DateOfBirth = new DateTime(1992, 3, 5),
                Phone = "0767882931",
                IsActive = false,
                CreateDate = DateTime.Now.AddYears(-1),
                TestTakenDate = DateTime.Now.AddDays(-45),
                TargetScore = "7.5"
            }
        };

        await dbContext.Users.AddRangeAsync(users);
        await dbContext.SaveChangesAsync();
    }

    //  Summary:
    //      Seeding tags 
    private async Task SeedTagAsync()
    {
        List<Tag> tags = new()
        {
            new() { TagName = Enum.Tag.IeltsAcademic.GetDescription() },
            new() { TagName = Enum.Tag.IeltsGeneral.GetDescription() },
            new() { TagName = Enum.Tag.Reading.GetDescription() },
            new() { TagName = Enum.Tag.Listening.GetDescription() },
            new() { TagName = Enum.Tag.Writing.GetDescription() },
            new() { TagName = Enum.Tag.Speaking.GetDescription() }
        };

        await dbContext.Tags.AddRangeAsync(tags);
        await dbContext.SaveChangesAsync();
    }

    //  Summary:
    //      Seeding Test Categories
    private async Task SeedTestCategoryAsync()
    {
        List<TestCategory> categories = new()
        {
            new() { TestCategoryName = Enum.TestCategory.IeltsAcademic.GetDescription() },
            new() { TestCategoryName = Enum.TestCategory.IeltsGeneral.GetDescription() }
        };

        await dbContext.TestCategories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }

    //  Summary:
    //      Seeding Test
    private async Task SeedTestAsync()
    {
        if (!dbContext.TestCategories.Any())
        {
            Console.WriteLine("Not found any test category to seed data for test.");
            return;
        }

        // Get all test category
        var testCategories = await dbContext.TestCategories.ToListAsync();

        // Ielts Academic
        var ieltsAcademicCategory = testCategories.FirstOrDefault(x =>
            x.TestCategoryName!.Equals(Enum.TestCategory.IeltsAcademic.GetDescription()));

        // Ielts General 
        var ieltsGeneralCategory = testCategories.FirstOrDefault(x =>
            x.TestCategoryName!.Equals(Enum.TestCategory.IeltsGeneral.GetDescription()));

        // Get all tag
        var tags = await dbContext.Tags.ToListAsync();

        // Get all users
        var users = await dbContext.Users.Take(10).ToListAsync();

        // Init random
        var rnd = new Random();

        // Generate list of Test
        List<Test> tests = new()
        {
            new()
            {
                TestTitle = "IELTS Simulation Reading Test 1",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Reading Test 2",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Reading Test 3",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Listening Test 1",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now,
                Comments = new List<Comment>()
                {
                    new()
                    {
                        UserId = users[0].UserId,
                        Content = "30/40, bài Listening này khó thật",
                        CommentDate = DateTime.Now,
                        TotalChildNode = 2,
                        InverseParentComment = new List<Comment>()
                        {
                            new()
                            {
                                UserId = users[1].UserId,
                                Content = "Mình cũng 30/40",
                                CommentDate = DateTime.Now.AddSeconds(1000),
                            },
                            new()
                            {
                                UserId = users[2].UserId,
                                Content = "Sao câu 23 đáp án ABC vậy ạ ???",
                                CommentDate = DateTime.Now.AddDays(1),
                            }
                        }
                    },
                    new()
                    {
                        UserId = users[3].UserId,
                        Content = "33/40, part 4 hơi khó",
                        CommentDate = DateTime.Now.AddDays(2),
                        TotalChildNode = 0
                    }
                },
                TestSections = new List<TestSection>()
                {
                    new()
                    {
                        TestSectionName = "Recording 1",
                        AudioResourceUrl = "http://example.com/audio1.mp3",
                        TotalQuestion = 10,
                        SectionTranscript = "This section includes various passages...",
                        TestSectionPartitions = new List<TestSectionPartition>()
                        {
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag()
                                    { PartitionTagDesc = "[Listening] Note/Form Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 1,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 2,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 3,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "B", AnswerDisplay = "B" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 4,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 5,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 6,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        AudioResourceUrl = "http://example.com/audio2.mp3",
                        TotalQuestion = 10,
                        SectionTranscript = "This section will test your listening skills...",
                        TestSectionPartitions = new List<TestSectionPartition>()
                        {
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Table Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 7,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 8,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 9,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "B", AnswerDisplay = "B" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 10,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 11,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 12,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    }
                                }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Multiple Choice" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 13,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 1",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" },
                                            new() { IsTrue = false, AnswerText = "B", AnswerDisplay = "B" },
                                            new() { IsTrue = false, AnswerText = "C", AnswerDisplay = "C" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 14,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 2",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "A", AnswerDisplay = "A" },
                                            new() { IsTrue = true, AnswerText = "B", AnswerDisplay = "B" },
                                            new() { IsTrue = false, AnswerText = "C", AnswerDisplay = "C" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 15,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "A", AnswerDisplay = "A" },
                                            new() { IsTrue = false, AnswerText = "B", AnswerDisplay = "B" },
                                            new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        AudioResourceUrl = "http://example.com/audio3.mp3",
                        TotalQuestion = 10,
                        SectionTranscript = "Focus on grammar rules and vocabulary usage...",
                        TestSectionPartitions = new List<TestSectionPartition>()
                        {
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag()
                                    { PartitionTagDesc = "[Listening] Note/Form Completion" }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Table Completion" }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        AudioResourceUrl = "http://example.com/audio4.mp3",
                        TotalQuestion = 10,
                        SectionTranscript = "This section requires you to write essays...",
                        TestSectionPartitions = new List<TestSectionPartition>()
                        {
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag()
                                    { PartitionTagDesc = "[Listening] Summary/Flow chart Completion" }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Matching" }
                            }
                        }
                    }
                }
            },
            new()
            {
                TestTitle = "IELTS Simulation Listening Test 2",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Listening Test 3",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Writing Test 1",
                Duration = 2400,
                TestType = Enum.TestType.Writing.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Writing.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Writing Test 2",
                Duration = 2400,
                TestType = Enum.TestType.Writing.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Writing.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Listening Test 4",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            },
            new()
            {
                TestTitle = "IELTS Simulation Reading Test 4",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = 0,
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now
            }
        };

        // Add range & Save change db
        await dbContext.Tests.AddRangeAsync(tests);
        await dbContext.SaveChangesAsync();
    }


    //  Summary:
    //      Seeding Test History
    private async Task SeedTestHistoryAsync()
    {
        var rnd = new Random();
        var users = await dbContext.Users.OrderBy(x => x.Id).Take(5).ToListAsync();
        var tests = await dbContext.Tests.Include(x => x.TestCategory).Take(10).ToListAsync();

        List<TestHistory> testHistories = new();
        for (int i = 0; i < 5; i++)
        {
            var rndUser = users[rnd.Next(users.Count)];
            var rndTest = tests[rnd.Next(tests.Count)];

            // if (rndTest.TestTitle.Contains("Listening 1"))
            // {
            //     var firstTestSection = await dbContext.TestSections.Where(x => 
            //         x.TestId.ToString().Equals(rndTest.TestId))
            //         .Include(x => x.TestSectionPartitions)
            //         .FirstOrDefaultAsync();
            // }
            //

            testHistories.Add(new()
            {
                TakenDate = DateTime.Now.AddDays(rnd.Next(-100, 100)),
                TotalCompletionTime = rnd.Next(3600),
                TestType = rndTest.TestType,
                IsFull = rnd.Next(0, 1) == 1,
                TestCategoryId = rndTest.TestCategoryId,
                UserId = rndUser.UserId,
                TestId = rndTest.TestId
            });
        }

        var listeningTest1 = await dbContext.Tests
            .FirstOrDefaultAsync(x => 
                x.TestTitle.Contains("Listening Test 1"));
        
         listeningTest1!.TestHistories.Add(new()
            {
                TakenDate = DateTime.Now.AddDays(rnd.Next(-100,100)),
                TotalCompletionTime = rnd.Next(3600),
                TestType = listeningTest1.TestType,
                IsFull = rnd.Next(0,1) == 1,
                TestCategoryId = listeningTest1.TestCategoryId,
                UserId = users[0].UserId,
                TestId = listeningTest1.TestId,
                PartitionHistories = new List<PartitionHistory>()
                {
                    new()
                    {
                        TestSectionName = "Recording 1",
                        TotalRightAnswer = 8,
                        TotalWrongAnswer = 2,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10,
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        TotalRightAnswer = 7,
                        TotalWrongAnswer = 3,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        TotalRightAnswer = 6,
                        TotalWrongAnswer = 4,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        TotalRightAnswer = 5,
                        TotalWrongAnswer = 5,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        TotalRightAnswer = 9,
                        TotalWrongAnswer = 1,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        TotalRightAnswer = 8,
                        TotalWrongAnswer = 2,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        TotalRightAnswer = 7,
                        TotalWrongAnswer = 2,
                        TotalSkipAnswer = 1,
                        TotalQuestion = 10
                    }
                }
            });

        await dbContext.TestHistories.AddRangeAsync(testHistories);
        await dbContext.SaveChangesAsync();
    }
}