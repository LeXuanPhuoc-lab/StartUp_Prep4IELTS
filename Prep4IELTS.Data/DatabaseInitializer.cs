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
            // Check whether the database exists and can be connected to
            if (!await dbContext.Database.CanConnectAsync())
            {
                // Check for applied migrations
                var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
                if (appliedMigrations.Any())
                {
                    Console.WriteLine("Migrations have been applied.");
                    return;
                }

                // Perform migration if necessary
                await dbContext.Database.MigrateAsync();
                Console.WriteLine("Database initialized successfully");
            }
            else
            {
                Console.WriteLine("Database cannot be connected to.");
            }
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
            // Test Band Score
            if (!dbContext.ScoreCalculations.Any()) await SeedBandScoreAsync();
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
            // Tests Grade
            if (!dbContext.TestGrades.Any()) await SeedTestGradeAsync();

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
    //      Seeding Band Score
    private async Task SeedBandScoreAsync()
    {
        List<ScoreCalculation> scoreCalculations = new()
        {
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 4,
                ToTotalRight = 5,
                BandScore = "2.5"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 6,
                ToTotalRight = 7,
                BandScore = "3"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 8,
                ToTotalRight = 9,
                BandScore = "3.5"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 10,
                ToTotalRight = 12,
                BandScore = "4"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 13,
                ToTotalRight = 14,
                BandScore = "4.5"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 15,
                ToTotalRight = 18,
                BandScore = "5.0"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 19,
                ToTotalRight = 22,
                BandScore = "5.5"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 23,
                ToTotalRight = 26,
                BandScore = "6.0"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 27,
                ToTotalRight = 29,
                BandScore = "6.5"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 30,
                ToTotalRight = 32,
                BandScore = "7.0"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 33,
                ToTotalRight = 34,
                BandScore = "7.5"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 35,
                ToTotalRight = 36,
                BandScore = "8.0"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 37,
                ToTotalRight = 38,
                BandScore = "8.5"
            },
            new()
            {
                TestType = Enum.TestType.Listening.GetDescription(),
                FromTotalRight = 39,
                ToTotalRight = 40,
                BandScore = "9.0"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 6,
                ToTotalRight = 8,
                BandScore = "2.5"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 11,
                ToTotalRight = 9,
                BandScore = "3.0"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 12,
                ToTotalRight = 14,
                BandScore = "3.5"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 15,
                ToTotalRight = 18,
                BandScore = "4.0"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 19,
                ToTotalRight = 22,
                BandScore = "4.5"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 23,
                ToTotalRight = 26,
                BandScore = "5.0"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 27,
                ToTotalRight = 29,
                BandScore = "5.5"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 30,
                ToTotalRight = 31,
                BandScore = "6.0"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 32,
                ToTotalRight = 33,
                BandScore = "6.5"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 34,
                ToTotalRight = 35,
                BandScore = "7.0"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 36,
                ToTotalRight = 36,
                BandScore = "7.5"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 37,
                ToTotalRight = 38,
                BandScore = "8.0"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 39,
                ToTotalRight = 39,
                BandScore = "8.5"
            },
            new()
            {
                TestType = Enum.TestType.Reading.GetDescription(),
                FromTotalRight = 40,
                ToTotalRight = 40,
                BandScore = "9.0"
            }
        };

        await dbContext.ScoreCalculations.AddRangeAsync(scoreCalculations);
        await dbContext.SaveChangesAsync();
    }
    
    //  Summary:
    //      Seeding user
    private async Task SeedUserAsync()
    {
        var rnd = new Random();
        List<SystemRole> roles = await dbContext.SystemRoles.ToListAsync();
        
        List<User> users = new()
        {
            new User
            {
                UserId = Guid.Parse("ee3da1ec-e2d1-458c-b6c9-23e76727da8d"),
                ClerkId = "user_2ksTPdbJa8xQxojVQtM5FSPb5s1",
                FirstName = "Admin",
                LastName = "Prep4Ielts",
                Email = "admin.prep4ielts@gmail.com",
                DateOfBirth = new DateTime(1990, 5, 15),
                Phone = "0978112391",
                IsActive = true,
                CreateDate = DateTime.Now,
                TestTakenDate = DateTime.Now.AddDays(-30),
                TargetScore = "8.0",
                RoleId = roles.First(x => x.RoleName!.Equals(Enum.SystemRole.Admin.GetDescription())).RoleId
            },
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
                TargetScore = "8.0",
                RoleId = roles[rnd.Next(roles.Count())].RoleId
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
                TargetScore = "6.0",
                RoleId = roles[rnd.Next(roles.Count())].RoleId
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
                TargetScore = "5.5",
                RoleId = roles[rnd.Next(roles.Count())].RoleId
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
                TargetScore = "6.5",
                RoleId = roles[rnd.Next(roles.Count())].RoleId
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
                TargetScore = "7.5",
                RoleId = roles[rnd.Next(roles.Count())].RoleId
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
        var users = await dbContext.Users.OrderBy(x => x.Id).Take(10).ToListAsync();

        // Init random
        var rnd = new Random();

        // Generate list of Test
        List<Test> tests = new()
        {
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Reading Test 1",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Reading Test 2",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Reading Test 3",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Listening Test 1",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100)),
                Comments = new List<Comment>()
                {
                    new()
                    {
                        UserId = users[0].UserId,
                        Content = "30/40, bài Listening này khó thật",
                        CommentDate = DateTime.Now,
                        TotalChildNode = 3,
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
                                TotalChildNode = 2,
                                InverseParentComment = new List<Comment>()
                                {
                                    new()
                                    {
                                        UserId = users[1].UserId,
                                        Content = "Mình cũng sai câu này :>>>",
                                        CommentDate = DateTime.Now.AddSeconds(1000),
                                    },
                                    new()
                                    {
                                        UserId = users[4].UserId,
                                        Content = "Coi transcript ở đây: https://pre4ielts.com/tests/transcript",
                                        CommentDate = DateTime.Now.AddSeconds(1000),
                                    }
                                }
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
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio1.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
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
                                        {
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH OF JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "JULY 26",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "JULY 26TH",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26 JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            }
                                        }
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
                                    },
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
                                            { new() { IsTrue = true, AnswerText = "B", AnswerDisplay = "B" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 9,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 10,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio2.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
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
                                        QuestionNumber = 11,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 12,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 13,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 14,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 15,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
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
                                        QuestionNumber = 16,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 1",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = true, AnswerText = "The Earth revolves around the Sun", AnswerDisplay = "The Earth revolves around the Sun" },
                                            new() { IsTrue = false, AnswerText = "The Sun revolves around the Earth", AnswerDisplay = "The Sun revolves around the Earth" },
                                            new() { IsTrue = false, AnswerText = "The Earth is stationary", AnswerDisplay = "The Earth is stationary" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 17,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 2",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "Water boils at 50 degrees Celsius", AnswerDisplay = "Water boils at 50 degrees Celsius" },
                                            new() { IsTrue = true, AnswerText = "Water boils at 100 degrees Celsius", AnswerDisplay = "Water boils at 100 degrees Celsius" },
                                            new() { IsTrue = false, AnswerText = "Water boils at 150 degrees Celsius", AnswerDisplay = "Water boils at 150 degrees Celsius" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 18,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "The capital of France is Berlin", AnswerDisplay = "The capital of France is Berlin" },
                                            new() { IsTrue = true, AnswerText = "The capital of France is Paris", AnswerDisplay = "The capital of France is Paris" },
                                            new() { IsTrue = false, AnswerText = "The capital of France is Madrid", AnswerDisplay = "The capital of France is Madrid" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 19,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = true, AnswerText = "Photosynthesis occurs in plants", AnswerDisplay = "Photosynthesis occurs in plants" },
                                            new() { IsTrue = false, AnswerText = "Photosynthesis occurs in animals", AnswerDisplay = "Photosynthesis occurs in animals" },
                                            new() { IsTrue = false, AnswerText = "Photosynthesis occurs in fungi", AnswerDisplay = "Photosynthesis occurs in fungi" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 20,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "The speed of light is 300,000 km/s", AnswerDisplay = "The speed of light is 300,000 km/s" },
                                            new() { IsTrue = true, AnswerText = "The speed of light is 299,792 km/s", AnswerDisplay = "The speed of light is 299,792 km/s" },
                                            new() { IsTrue = false, AnswerText = "The speed of light is 150,000 km/s", AnswerDisplay = "The speed of light is 150,000 km/s" }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio3.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
                        TotalQuestion = 10,
                        SectionTranscript = "Focus on grammar rules and vocabulary usage...",
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
                                        QuestionNumber = 21,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 22,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 23,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 24,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 25,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Table Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 26,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 27,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 28,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 29,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 30,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio4.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
                        TotalQuestion = 10,
                        SectionTranscript = "This section requires you to write essays...",
                        TestSectionPartitions = new List<TestSectionPartition>()
                        {
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag()
                                    { PartitionTagDesc = "[Listening] Summary/Flow chart Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 31,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 32,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 33,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 34,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 35,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Matching" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 36,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 37,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 38,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 39,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 40,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Listening Test 2",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100)),
                TestSections = new List<TestSection>()
                {
                    new()
                    {
                        TestSectionName = "Recording 1",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio1.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
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
                                        {
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH OF JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "JULY 26",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "JULY 26TH",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26 JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            }
                                        }
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
                                    },
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
                                            { new() { IsTrue = true, AnswerText = "B", AnswerDisplay = "B" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 9,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 10,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio2.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
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
                                        QuestionNumber = 11,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 12,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 13,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 14,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 15,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
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
                                        QuestionNumber = 16,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 1",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = true, AnswerText = "The Earth revolves around the Sun", AnswerDisplay = "The Earth revolves around the Sun" },
                                            new() { IsTrue = false, AnswerText = "The Sun revolves around the Earth", AnswerDisplay = "The Sun revolves around the Earth" },
                                            new() { IsTrue = false, AnswerText = "The Earth is stationary", AnswerDisplay = "The Earth is stationary" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 17,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 2",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "Water boils at 50 degrees Celsius", AnswerDisplay = "Water boils at 50 degrees Celsius" },
                                            new() { IsTrue = true, AnswerText = "Water boils at 100 degrees Celsius", AnswerDisplay = "Water boils at 100 degrees Celsius" },
                                            new() { IsTrue = false, AnswerText = "Water boils at 150 degrees Celsius", AnswerDisplay = "Water boils at 150 degrees Celsius" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 18,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "The capital of France is Berlin", AnswerDisplay = "The capital of France is Berlin" },
                                            new() { IsTrue = true, AnswerText = "The capital of France is Paris", AnswerDisplay = "The capital of France is Paris" },
                                            new() { IsTrue = false, AnswerText = "The capital of France is Madrid", AnswerDisplay = "The capital of France is Madrid" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 19,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = true, AnswerText = "Photosynthesis occurs in plants", AnswerDisplay = "Photosynthesis occurs in plants" },
                                            new() { IsTrue = false, AnswerText = "Photosynthesis occurs in animals", AnswerDisplay = "Photosynthesis occurs in animals" },
                                            new() { IsTrue = false, AnswerText = "Photosynthesis occurs in fungi", AnswerDisplay = "Photosynthesis occurs in fungi" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 20,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "The speed of light is 300,000 km/s", AnswerDisplay = "The speed of light is 300,000 km/s" },
                                            new() { IsTrue = true, AnswerText = "The speed of light is 299,792 km/s", AnswerDisplay = "The speed of light is 299,792 km/s" },
                                            new() { IsTrue = false, AnswerText = "The speed of light is 150,000 km/s", AnswerDisplay = "The speed of light is 150,000 km/s" }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio3.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
                        TotalQuestion = 10,
                        SectionTranscript = "Focus on grammar rules and vocabulary usage...",
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
                                        QuestionNumber = 21,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 22,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 23,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 24,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 25,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Table Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 26,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 27,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 28,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 29,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 30,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio4.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
                        TotalQuestion = 10,
                        SectionTranscript = "This section requires you to write essays...",
                        TestSectionPartitions = new List<TestSectionPartition>()
                        {
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag()
                                    { PartitionTagDesc = "[Listening] Summary/Flow chart Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 31,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 32,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 33,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 34,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 35,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Matching" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 36,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 37,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 38,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 39,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 40,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Listening Test 3",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100)),
                TestSections = new List<TestSection>()
                {
                    new()
                    {
                        TestSectionName = "Recording 1",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio1.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
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
                                        {
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26TH OF JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "JULY 26",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "JULY 26TH",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            },
                                            new()
                                            {
                                                IsTrue = true,
                                                AnswerText = "26 JULY",
                                                AnswerDisplay = "26TH (OF) JULY [OR] JULY 26(TH) [OR] 26 JULY"
                                            }
                                        }
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
                                    },
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
                                            { new() { IsTrue = true, AnswerText = "B", AnswerDisplay = "B" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 9,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 10,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio2.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
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
                                        QuestionNumber = 11,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 12,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 13,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 14,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 15,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
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
                                        QuestionNumber = 16,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 1",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = true, AnswerText = "The Earth revolves around the Sun", AnswerDisplay = "The Earth revolves around the Sun" },
                                            new() { IsTrue = false, AnswerText = "The Sun revolves around the Earth", AnswerDisplay = "The Sun revolves around the Earth" },
                                            new() { IsTrue = false, AnswerText = "The Earth is stationary", AnswerDisplay = "The Earth is stationary" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 17,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 2",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "Water boils at 50 degrees Celsius", AnswerDisplay = "Water boils at 50 degrees Celsius" },
                                            new() { IsTrue = true, AnswerText = "Water boils at 100 degrees Celsius", AnswerDisplay = "Water boils at 100 degrees Celsius" },
                                            new() { IsTrue = false, AnswerText = "Water boils at 150 degrees Celsius", AnswerDisplay = "Water boils at 150 degrees Celsius" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 18,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "The capital of France is Berlin", AnswerDisplay = "The capital of France is Berlin" },
                                            new() { IsTrue = true, AnswerText = "The capital of France is Paris", AnswerDisplay = "The capital of France is Paris" },
                                            new() { IsTrue = false, AnswerText = "The capital of France is Madrid", AnswerDisplay = "The capital of France is Madrid" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 19,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = true, AnswerText = "Photosynthesis occurs in plants", AnswerDisplay = "Photosynthesis occurs in plants" },
                                            new() { IsTrue = false, AnswerText = "Photosynthesis occurs in animals", AnswerDisplay = "Photosynthesis occurs in animals" },
                                            new() { IsTrue = false, AnswerText = "Photosynthesis occurs in fungi", AnswerDisplay = "Photosynthesis occurs in fungi" }
                                        }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 20,
                                        IsMultipleChoice = true,
                                        QuestionDesc = "Multiple Choice 3",
                                        QuestionAnswers = new List<QuestionAnswer>()
                                        {
                                            new() { IsTrue = false, AnswerText = "The speed of light is 300,000 km/s", AnswerDisplay = "The speed of light is 300,000 km/s" },
                                            new() { IsTrue = true, AnswerText = "The speed of light is 299,792 km/s", AnswerDisplay = "The speed of light is 299,792 km/s" },
                                            new() { IsTrue = false, AnswerText = "The speed of light is 150,000 km/s", AnswerDisplay = "The speed of light is 150,000 km/s" }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio3.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
                        TotalQuestion = 10,
                        SectionTranscript = "Focus on grammar rules and vocabulary usage...",
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
                                        QuestionNumber = 21,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 22,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 23,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 24,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 25,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Table Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 26,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 27,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 28,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 29,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 30,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        CloudResource = new CloudResource()
                        {
                            Url = "http://example.com/audio4.mp3",
                            CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
                        },
                        TotalQuestion = 10,
                        SectionTranscript = "This section requires you to write essays...",
                        TestSectionPartitions = new List<TestSectionPartition>()
                        {
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag()
                                    { PartitionTagDesc = "[Listening] Summary/Flow chart Completion" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 31,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 32,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 33,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 34,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 35,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            },
                            new()
                            {
                                PartitionDesc = "Description for questions",
                                PartitionTag = new PartitionTag() { PartitionTagDesc = "[Listening] Matching" },
                                Questions = new List<Question>()
                                {
                                    new()
                                    {
                                        QuestionNumber = 36,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 37,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "A", AnswerDisplay = "A" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 38,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 39,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "C", AnswerDisplay = "C" } }
                                    },
                                    new()
                                    {
                                        QuestionNumber = 40,
                                        QuestionAnswers = new List<QuestionAnswer>()
                                            { new() { IsTrue = true, AnswerText = "D", AnswerDisplay = "D" } }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Writing Test 1",
                Duration = 2400,
                TestType = Enum.TestType.Writing.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Writing.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Writing Test 2",
                Duration = 2400,
                TestType = Enum.TestType.Writing.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Writing.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Listening Test 4",
                Duration = 2400,
                TestType = Enum.TestType.Listening.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Listening.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
            },
            new()
            {
                TestId = Guid.NewGuid(),
                TestTitle = "IELTS Simulation Reading Test 4",
                Duration = 2400,
                TestType = Enum.TestType.Reading.GetDescription(),
                TotalQuestion = 40,
                TotalSection = 4,
                TotalEngaged = rnd.Next(0, 200_000),
                TestCategoryId = ieltsAcademicCategory?.TestCategoryId ?? 0,
                Tags = tags.Where(x => x.TagName != null && (
                    x.TagName.Equals(Enum.Tag.IeltsAcademic.GetDescription()) ||
                    x.TagName.Equals(Enum.Tag.Reading.GetDescription()
                    ))).ToList(),
                CreateDate = DateTime.Now.AddDays(rnd.Next(-100, 100))
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
        var tests = await dbContext.Tests
            .OrderBy(x => x.Id).Take(10)
            .Select(x => new Test()
            {
                TestId = x.TestId,
                TestType = x.TestType,
                TestCategoryId = x.TestCategoryId
            })
            .ToListAsync();
        var scoreCalculations = await dbContext.ScoreCalculations.ToListAsync();

        List<TestHistory> testHistories = new();
        for (int i = 0; i < 5; i++)
        {
            var rndUser = users[rnd.Next(users.Count)];
            var rndTest = tests[rnd.Next(tests.Count)];
            var rndBandScore = scoreCalculations[rnd.Next(scoreCalculations.Count)];
            
            testHistories.Add(new()
            {
                TakenDate = DateTime.Now.AddDays(rnd.Next(-100, 100)),
                TotalCompletionTime = rnd.Next(3600),
                TestType = rndTest.TestType,
                IsFull = rnd.Next(0, 1) == 1,
                TestCategoryId = rndTest.TestCategoryId,
                UserId = rndUser.UserId,
                TestId = rndTest.TestId,
                BandScore = rndBandScore.BandScore,
                TotalRightAnswer = rnd.Next(0, 10),
                TotalSkipAnswer = rnd.Next(0, 10),
                TotalWrongAnswer = rnd.Next(0, 10),
                AccuracyRate = 0.375,
                TotalQuestion = 40
            });
        }


        var listeningTest1 = await dbContext.Tests
            .Where(x => x.TestTitle.Contains("Listening Test 1"))
            .Include(x => x.TestSections)
            .ThenInclude(x => x.TestSectionPartitions)
            .AsSplitQuery()
            .FirstOrDefaultAsync();

        var testSections = listeningTest1!.TestSections.ToList();

        for (int i = 0; i < 2; ++i)
        {
            listeningTest1!.TestHistories.Add(new()
            {
                TakenDate = DateTime.Now.AddDays(rnd.Next(-100, 100)),
                TotalCompletionTime = rnd.Next(3600),
                TotalRightAnswer = 26,
                TotalWrongAnswer = 13,
                TotalSkipAnswer = 1,
                TotalQuestion = 40,
                AccuracyRate = double.Parse("0.65"),
                TestType = listeningTest1.TestType,
                BandScore = scoreCalculations[rnd.Next(scoreCalculations.Count)].BandScore,
                IsFull = rnd.Next(0, 1) == 1,
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
                        AccuracyRate = double.Parse("0.6"),
                        TestSectionPartId = testSections[0].TestSectionPartitions.First().TestSectionPartId
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        TotalRightAnswer = 7,
                        TotalWrongAnswer = 3,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10,
                        AccuracyRate = double.Parse("0.375"),
                        TestSectionPartId = testSections[1].TestSectionPartitions.ToList()[0].TestSectionPartId
                    },
                    new()
                    {
                        TestSectionName = "Recording 2",
                        TotalRightAnswer = 6,
                        TotalWrongAnswer = 4,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10,
                        AccuracyRate = double.Parse("0.2"),
                        TestSectionPartId = testSections[1].TestSectionPartitions.ToList()[1].TestSectionPartId
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        TotalRightAnswer = 5,
                        TotalWrongAnswer = 5,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10,
                        AccuracyRate = double.Parse("0.4"),
                        TestSectionPartId = testSections[2].TestSectionPartitions.ToList()[0].TestSectionPartId
                    },
                    new()
                    {
                        TestSectionName = "Recording 3",
                        TotalRightAnswer = 9,
                        TotalWrongAnswer = 1,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10,
                        AccuracyRate = double.Parse("0.8"),
                        TestSectionPartId = testSections[2].TestSectionPartitions.ToList()[1].TestSectionPartId
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        TotalRightAnswer = 8,
                        TotalWrongAnswer = 2,
                        TotalSkipAnswer = 0,
                        TotalQuestion = 10,
                        AccuracyRate = double.Parse("0.1"),
                        TestSectionPartId = testSections[3].TestSectionPartitions.ToList()[0].TestSectionPartId
                    },
                    new()
                    {
                        TestSectionName = "Recording 4",
                        TotalRightAnswer = 7,
                        TotalWrongAnswer = 2,
                        TotalSkipAnswer = 1,
                        TotalQuestion = 10,
                        AccuracyRate = double.Parse("0.00"),
                        TestSectionPartId = testSections[3].TestSectionPartitions.ToList()[1].TestSectionPartId
                    }
                }
            });
        }

        await dbContext.TestHistories.AddRangeAsync(testHistories);
        await dbContext.SaveChangesAsync();
    }
    
    //  Summary:
    //      Seeding Test Grade
    private async Task SeedTestGradeAsync()
    {
        var test = await dbContext.Tests
            .AsSplitQuery()
            .Include(x => x.TestSections)
            .ThenInclude(x => x.TestSectionPartitions)
            .ThenInclude(x => x.Questions)
            .FirstOrDefaultAsync(tst => 
                tst.TestTitle.Contains("Listening Test 1"));

        if (test == null)
        {
            Console.WriteLine("Not found Listening Test 1 to grade");
            return;
        }
        
        var testHistory = await dbContext.TestHistories
            .AsSplitQuery()
            .Include(x => x.PartitionHistories)
            .FirstOrDefaultAsync(x => 
                x.TestId.Equals(test.TestId));

        var questions = test.TestSections
            .SelectMany(x => x.TestSectionPartitions)
            .SelectMany(x => x.Questions)
            .ToList();

        var partitionHistories = testHistory!.PartitionHistories.ToList();

        List<TestGrade> testGrades = new()
        {
            new()
            {
                QuestionNumber = 1,
                QuestionId = questions[0].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "A",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 2,
                QuestionId = questions[1].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "B",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 3,
                QuestionId = questions[2].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "C",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 4,
                QuestionId = questions[3].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "D",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 5,
                QuestionId = questions[4].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "May 26th",
                InputedAnswer = "May 26th",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 6,
                QuestionId = questions[5].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "F",
                InputedAnswer = "",
                GradeStatus = Enum.GradeStatus.Skip.GetDescription()
            },
            new()
            {
                QuestionNumber = 7,
                QuestionId = questions[6].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 8,
                QuestionId = questions[7].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "A",
                InputedAnswer = "A",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 9,
                QuestionId = questions[8].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "A",
                InputedAnswer = "",
                GradeStatus = Enum.GradeStatus.Skip.GetDescription()
            },
            new()
            {
                QuestionNumber = 10,
                QuestionId = questions[9].QuestionId,
                PartitionHistoryId = partitionHistories[0].PartitionHistoryId,
                RightAnswer = "G",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 11,
                QuestionId = questions[10].QuestionId,
                PartitionHistoryId = partitionHistories[1].PartitionHistoryId,
                RightAnswer = "G",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 12,
                QuestionId = questions[11].QuestionId,
                PartitionHistoryId = partitionHistories[1].PartitionHistoryId,
                RightAnswer = "G",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 13,
                QuestionId = questions[12].QuestionId,
                PartitionHistoryId = partitionHistories[1].PartitionHistoryId,
                RightAnswer = "G",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 14,
                QuestionId = questions[13].QuestionId,
                PartitionHistoryId = partitionHistories[1].PartitionHistoryId,
                RightAnswer = "A",
                InputedAnswer = "A",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 15,
                QuestionId = questions[14].QuestionId,
                PartitionHistoryId = partitionHistories[1].PartitionHistoryId,
                RightAnswer = "G",
                InputedAnswer = "G",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 16,
                QuestionId = questions[15].QuestionId,
                PartitionHistoryId = partitionHistories[2].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 17,
                QuestionId = questions[16].QuestionId,
                PartitionHistoryId = partitionHistories[2].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 18,
                QuestionId = questions[17].QuestionId,
                PartitionHistoryId = partitionHistories[2].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 19,
                QuestionId = questions[18].QuestionId,
                PartitionHistoryId = partitionHistories[2].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 20,
                QuestionId = questions[19].QuestionId,
                PartitionHistoryId = partitionHistories[2].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 21,
                QuestionId = questions[20].QuestionId,
                PartitionHistoryId = partitionHistories[3].PartitionHistoryId,
                RightAnswer = "G",
                InputedAnswer = "G",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 22,
                QuestionId = questions[21].QuestionId,
                PartitionHistoryId = partitionHistories[3].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 23,
                QuestionId = questions[22].QuestionId,
                PartitionHistoryId = partitionHistories[3].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 24,
                QuestionId = questions[23].QuestionId,
                PartitionHistoryId = partitionHistories[3].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 25,
                QuestionId = questions[24].QuestionId,
                PartitionHistoryId = partitionHistories[3].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 26,
                QuestionId = questions[25].QuestionId,
                PartitionHistoryId = partitionHistories[4].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 27,
                QuestionId = questions[26].QuestionId,
                PartitionHistoryId = partitionHistories[4].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 28,
                QuestionId = questions[27].QuestionId,
                PartitionHistoryId = partitionHistories[4].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 29,
                QuestionId = questions[28].QuestionId,
                PartitionHistoryId = partitionHistories[4].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 30,
                QuestionId = questions[29].QuestionId,
                PartitionHistoryId = partitionHistories[4].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 31,
                QuestionId = questions[30].QuestionId,
                PartitionHistoryId = partitionHistories[5].PartitionHistoryId,
                RightAnswer = "A",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 32,
                QuestionId = questions[31].QuestionId,
                PartitionHistoryId = partitionHistories[5].PartitionHistoryId,
                RightAnswer = "B",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 33,
                QuestionId = questions[32].QuestionId,
                PartitionHistoryId = partitionHistories[5].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "C",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 34,
                QuestionId = questions[33].QuestionId,
                PartitionHistoryId = partitionHistories[5].PartitionHistoryId,
                RightAnswer = "D",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 35,
                QuestionId = questions[34].QuestionId,
                PartitionHistoryId = partitionHistories[5].PartitionHistoryId,
                RightAnswer = "May 26th",
                InputedAnswer = "May 26th",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 36,
                QuestionId = questions[35].QuestionId,
                PartitionHistoryId = partitionHistories[6].PartitionHistoryId,
                RightAnswer = "F",
                InputedAnswer = "",
                GradeStatus = Enum.GradeStatus.Skip.GetDescription()
            },
            new()
            {
                QuestionNumber = 37,
                QuestionId = questions[36].QuestionId,
                PartitionHistoryId = partitionHistories[6].PartitionHistoryId,
                RightAnswer = "C",
                InputedAnswer = "D",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            },
            new()
            {
                QuestionNumber = 38,
                QuestionId = questions[37].QuestionId,
                PartitionHistoryId = partitionHistories[6].PartitionHistoryId,
                RightAnswer = "A",
                InputedAnswer = "A",
                GradeStatus = Enum.GradeStatus.Correct.GetDescription()
            },
            new()
            {
                QuestionNumber = 39,
                QuestionId = questions[38].QuestionId,
                PartitionHistoryId = partitionHistories[6].PartitionHistoryId,
                RightAnswer = "A",
                InputedAnswer = "",
                GradeStatus = Enum.GradeStatus.Skip.GetDescription()
            },
            new()
            {
                QuestionNumber = 40,
                QuestionId = questions[39].QuestionId,
                PartitionHistoryId = partitionHistories[6].PartitionHistoryId,
                RightAnswer = "G",
                InputedAnswer = "B",
                GradeStatus = Enum.GradeStatus.Wrong.GetDescription()
            }
        };

        await dbContext.TestGrades.AddRangeAsync(testGrades);
        await dbContext.SaveChangesAsync();
    }
    
}