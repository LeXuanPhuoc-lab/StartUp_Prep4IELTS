using System;
using System.Collections.Generic;

namespace Prep4IELTS.Data.Entities;

public partial class ScoreCalculation
{
    public int ScoreCalculationId { get; set; }

    public int FromTotalRight { get; set; }

    public int ToTotalRight { get; set; }

    public string TestType { get; set; } = null!;

    public string BandScore { get; set; } = null!;

    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
}
