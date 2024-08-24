using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Context;

public partial class Prep4IeltsContext : DbContext
{
    public Prep4IeltsContext()
    {
    }

    public Prep4IeltsContext(DbContextOptions<Prep4IeltsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Flashcard> Flashcards { get; set; }

    public virtual DbSet<FlashcardDetail> FlashcardDetails { get; set; }

    public virtual DbSet<PartitionHistory> PartitionHistories { get; set; }

    public virtual DbSet<PartitionTag> PartitionTags { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    public virtual DbSet<SystemRole> SystemRoles { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestCategory> TestCategories { get; set; }

    public virtual DbSet<TestGrade> TestGrades { get; set; }

    public virtual DbSet<TestHistory> TestHistories { get; set; }

    public virtual DbSet<TestSection> TestSections { get; set; }

    public virtual DbSet<TestSectionPartition> TestSectionPartitions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());
    
    private string GetConnectionString()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? null!;

        if (!string.IsNullOrEmpty(environment))
        {
            builder.AddJsonFile($"appsettings.{environment}.json");
        }

        IConfiguration configuration = builder.Build();

        return configuration.GetConnectionString("DefaultConnectionString")!;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentDate)
                .HasColumnType("datetime")
                .HasColumnName("comment_date");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.ParentCommentId).HasColumnName("parent_comment_id");
            entity.Property(e => e.TestId).HasColumnName("test_id").IsRequired(false);
            entity.Property(e => e.TotalChildNode).HasColumnName("total_child_node");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK_Comment_ParentComment");

            entity.HasOne(d => d.Test).WithMany(p => p.Comments)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Test");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_User");
        });

        modelBuilder.Entity<Flashcard>(entity =>
        {
            entity.ToTable("Flashcard");

            entity.Property(e => e.FlashcardId).HasColumnName("flashcard_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.IsPublic).HasColumnName("isPublic");
            entity.Property(e => e.Title)
                .HasMaxLength(155)
                .HasColumnName("title");
            entity.Property(e => e.TotalView).HasColumnName("total_view");
            entity.Property(e => e.TotalWords).HasColumnName("total_words");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Flashcards)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Flashcard_User");
        });

        modelBuilder.Entity<FlashcardDetail>(entity =>
        {
            entity.HasKey(e => e.FlashcardDetailId).HasName("PK_FlashcardDetail");

            entity.ToTable("Flashcard_Detail");

            entity.Property(e => e.FlashcardDetailId).HasColumnName("flashcard_detail_id");
            entity.Property(e => e.Definition)
                .HasMaxLength(500)
                .HasColumnName("definition");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
            entity.Property(e => e.Example)
                .HasMaxLength(500)
                .HasColumnName("example");
            entity.Property(e => e.FlashcardId).HasColumnName("flashcard_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.WordForm)
                .HasMaxLength(50)
                .HasColumnName("word_form");
            entity.Property(e => e.WordPronunciation)
                .HasMaxLength(100)
                .HasColumnName("word_pronunciation");
            entity.Property(e => e.WordText)
                .HasMaxLength(100)
                .HasColumnName("word_text");

            entity.HasOne(d => d.Flashcard).WithMany(p => p.FlashcardDetails)
                .HasForeignKey(d => d.FlashcardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FlashcardDetail_Flashcard");
        });

        modelBuilder.Entity<PartitionHistory>(entity =>
        {
            entity.HasKey(e => e.PartitionHistoryId).HasName("PK_PartitionHistory");

            entity.ToTable("Partition_History");

            entity.Property(e => e.PartitionHistoryId).HasColumnName("partition_history_id");
            entity.Property(e => e.TestHistoryId).HasColumnName("test_history_id");
            entity.Property(e => e.TestSectionName)
                .HasMaxLength(50)
                .HasColumnName("test_section_name");
            entity.Property(e => e.TestSectionPartId).HasColumnName("test_section_part_id").IsRequired(false);
            entity.Property(e => e.TotalQuestion).HasColumnName("total_question");
            entity.Property(e => e.TotalRightAnswer).HasColumnName("total_right_answer");
            entity.Property(e => e.TotalSkipAnswer).HasColumnName("total_skip_answer");
            entity.Property(e => e.TotalWrongAnswer).HasColumnName("total_wrong_answer");

            entity.HasOne(d => d.TestHistory).WithMany(p => p.PartitionHistories)
                .HasForeignKey(d => d.TestHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartitionHistory_TestHistory");

            entity.HasOne(d => d.TestSectionPart).WithMany(p => p.PartitionHistories)
                .HasForeignKey(d => d.TestSectionPartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PartitionHistory_SectionPartition");
        });

        modelBuilder.Entity<PartitionTag>(entity =>
        {
            entity.HasKey(e => e.PartitionTagId).HasName("PK_PartitionTag");

            entity.ToTable("Partition_Tag");

            entity.Property(e => e.PartitionTagId).HasColumnName("partition_tag_id");
            entity.Property(e => e.PartitionTagDesc)
                .HasMaxLength(155)
                .HasColumnName("partition_tag_desc");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question");

            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.IsMultipleChoice).HasColumnName("isMultipleChoice");
            entity.Property(e => e.QuestionAnswerExplanation).HasColumnName("question_answer_explanation");
            entity.Property(e => e.QuestionDesc)
                .HasMaxLength(255)
                .HasColumnName("question_desc");
            entity.Property(e => e.QuestionNumber).HasColumnName("question_number");
            entity.Property(e => e.TestSectionPartId).HasColumnName("test_section_part_id");

            entity.HasOne(d => d.TestSectionPart).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestSectionPartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_TestSectionPartition");
        });

        modelBuilder.Entity<QuestionAnswer>(entity =>
        {
            entity.HasKey(e => e.QuestionAnswerId).HasName("PK_QuestionAnswer");

            entity.ToTable("Question_Answer");

            entity.Property(e => e.QuestionAnswerId).HasColumnName("question_answer_id");
            entity.Property(e => e.AnswerDisplay)
                .HasMaxLength(155)
                .HasColumnName("answer_display");
            entity.Property(e => e.AnswerText)
                .HasMaxLength(100)
                .HasColumnName("answer_text");
            entity.Property(e => e.IsTrue).HasColumnName("is_true");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionAnswer_Question");
        });

        modelBuilder.Entity<SystemRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_SystemRole");

            entity.ToTable("System_Role");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(155)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.TagName)
                .HasMaxLength(100)
                .HasColumnName("tag_name");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId);

            entity.ToTable("Test");

            entity.Property(e => e.TestId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("test_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.TestCategoryId).HasColumnName("test_category_id");
            entity.Property(e => e.TestTitle)
                .HasMaxLength(155)
                .HasColumnName("test_title");
            entity.Property(e => e.TestType)
                .HasMaxLength(50)
                .HasColumnName("test_type");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasColumnName("modified_date")
                .IsRequired(false);
            entity.Property(e => e.TotalEngaged).HasColumnName("total_engaged");
            entity.Property(e => e.TotalQuestion).HasColumnName("total_question");
            entity.Property(e => e.TotalSection).HasColumnName("total_section");

            entity.HasOne(d => d.TestCategory).WithMany(p => p.Tests)
                .HasForeignKey(d => d.TestCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Test_TestCategory");

            entity.HasMany(d => d.Tags).WithMany(p => p.Tests)
                .UsingEntity<Dictionary<string, object>>(
                    "TestTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TestTag_Tag"),
                    l => l.HasOne<Test>().WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TestTag_Test"),
                    j =>
                    {
                        j.HasKey("TestId", "TagId").HasName("PK_TestTag");
                        j.ToTable("Test_Tag");
                        j.IndexerProperty<Guid>("TestId").HasColumnName("test_id");
                        j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                    });
        });

        modelBuilder.Entity<TestCategory>(entity =>
        {
            entity.HasKey(e => e.TestCategoryId).HasName("PK_TestCategory");

            entity.ToTable("Test_Category");

            entity.Property(e => e.TestCategoryId).HasColumnName("test_category_id");
            entity.Property(e => e.TestCategoryName)
                .HasMaxLength(155)
                .HasColumnName("test_category_name");
        });

        modelBuilder.Entity<TestGrade>(entity =>
        {
            entity.HasKey(e => e.TestGradeId).HasName("PK_TestGrade");

            entity.ToTable("Test_Grade");

            entity.Property(e => e.TestGradeId).HasColumnName("test_grade_id");
            entity.Property(e => e.GradeStatus)
                .HasMaxLength(20)
                .HasColumnName("grade_status");
            entity.Property(e => e.InputedAnswer)
                .HasMaxLength(150)
                .HasColumnName("inputed_answer");
            entity.Property(e => e.PartitionHistoryId).HasColumnName("partition_history_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.QuestionNumber).HasColumnName("question_number");
            entity.Property(e => e.RightAnswer)
                .HasMaxLength(150)
                .HasColumnName("right_answer");

            entity.HasOne(d => d.PartitionHistory).WithMany(p => p.TestGrades)
                .HasForeignKey(d => d.PartitionHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestGrade_PartitionHistory");

            entity.HasOne(d => d.Question).WithMany(p => p.TestGrades)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestGrade_Question");
        });

        modelBuilder.Entity<TestHistory>(entity =>
        {
            entity.HasKey(e => e.TestHistoryId).HasName("PK_TestHistory");

            entity.ToTable("Test_History");

            entity.Property(e => e.TestHistoryId).HasColumnName("test_history_id");
            entity.Property(e => e.AccuracyRate).HasColumnName("accuracy_rate");
            entity.Property(e => e.BandScore)
                .HasMaxLength(50)
                .HasColumnName("band_score");
            entity.Property(e => e.IsFull).HasColumnName("isFull");
            entity.Property(e => e.TakenDate)
                .HasColumnType("datetime")
                .HasColumnName("taken_date");
            entity.Property(e => e.TestCategoryId).HasColumnName("test_category_id");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.TestType)
                .HasMaxLength(50)
                .HasColumnName("test_type");
            entity.Property(e => e.TotalCompletionTime).HasColumnName("total_completion_time");
            entity.Property(e => e.TotalQuestion).HasColumnName("total_question");
            entity.Property(e => e.TotalRightAnswer).HasColumnName("total_right_answer");
            entity.Property(e => e.TotalSkipAnswer).HasColumnName("total_skip_answer");
            entity.Property(e => e.TotalWrongAnswer).HasColumnName("total_wrong_answer");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.TestCategory).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.TestCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_TestCategory");

            entity.HasOne(d => d.Test).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_Test");

            entity.HasOne(d => d.User).WithMany(p => p.TestHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestHistory_User");
        });

        modelBuilder.Entity<TestSection>(entity =>
        {
            entity.HasKey(e => e.TestSectionId).HasName("PK_TestSection");

            entity.ToTable("Test_Section");

            entity.Property(e => e.TestSectionId).HasColumnName("test_section_id");
            entity.Property(e => e.AudioResourceUrl)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("audio_resource_url");
            entity.Property(e => e.ReadingDesc).HasColumnName("reading_desc");
            entity.Property(e => e.SectionTranscript).HasColumnName("section_transcript");
            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.TestSectionName)
                .HasMaxLength(50)
                .HasColumnName("test_section_name");
            entity.Property(e => e.TotalQuestion).HasColumnName("total_question");

            entity.HasOne(d => d.Test).WithMany(p => p.TestSections)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestSection_Test");
        });

        modelBuilder.Entity<TestSectionPartition>(entity =>
        {
            entity.HasKey(e => e.TestSectionPartId).HasName("PK_TestSectionPartition");

            entity.ToTable("Test_Section_Partition");

            entity.Property(e => e.TestSectionPartId).HasColumnName("test_section_part_id");
            entity.Property(e => e.IsVerticalLayout).HasColumnName("is_vertical_layout");
            entity.Property(e => e.PartitionDesc).HasColumnName("partition_desc");
            entity.Property(e => e.PartitionImage)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("partition_image");
            entity.Property(e => e.PartitionTagId).HasColumnName("partition_tag_id");
            entity.Property(e => e.TestSectionId).HasColumnName("test_section_id");

            entity.HasOne(d => d.PartitionTag).WithMany(p => p.TestSectionPartitions)
                .HasForeignKey(d => d.PartitionTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestSectionPartition_Tag");

            entity.HasOne(d => d.TestSection).WithMany(p => p.TestSectionPartitions)
                .HasForeignKey(d => d.TestSectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TestSectionPartition_TestSection");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("User");

            entity.HasIndex(e => e.ClerkId, "UQ__User__EA2ECA192588F56C").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newsequentialid())")
                .HasColumnName("user_id");
            entity.Property(e => e.AvatarImage)
                .HasMaxLength(2048)
                .IsUnicode(false)
                .HasColumnName("avatar_image");
            entity.Property(e => e.ClerkId)
                .HasMaxLength(155)
                .HasColumnName("clerk_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.TargetScore)
                .HasMaxLength(10)
                .HasColumnName("target_score");
            entity.Property(e => e.TestTakenDate)
                .HasColumnType("datetime")
                .HasColumnName("test_taken_date");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
