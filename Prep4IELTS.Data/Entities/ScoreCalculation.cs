using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public class ScoreCalculation
{
    public int ScoreCalculationId { get; set; }
    public int FromTotalRight { get; set; }
    public int ToTotalRight { get; set; }
    public string TestType { get; set; } = null!;
    public string BandScore { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<TestHistory> TestHistories { get; set; } = new List<TestHistory>();
}