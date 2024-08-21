using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class TestHistory
{
    public int TestHistoryId { get; set; }

    public int? TotalRightAnswer { get; set; }

    public int? TotalWrongAnswer { get; set; }

    public int? TotalSkipAnswer { get; set; }

    public int TotalQuestion { get; set; }

    public int TotalCompletionTime { get; set; }

    public DateTime TakenDate { get; set; }

    public double? AccuracyRate { get; set; }

    public bool IsFull { get; set; }

    public string TestType { get; set; } = null!;

    public string? BandScore { get; set; }

    public Guid UserId { get; set; }

    public Guid TestId { get; set; }

    public int TestCategoryId { get; set; }

    public virtual ICollection<PartitionHistory> PartitionHistories { get; set; } = new List<PartitionHistory>();

    public virtual Test Test { get; set; } = null!;

    public virtual TestCategory TestCategory { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
