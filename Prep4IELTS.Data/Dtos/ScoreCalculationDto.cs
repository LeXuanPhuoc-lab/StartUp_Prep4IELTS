using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record ScoreCalculationDto(
    int ScoreCalculationId, 
    int FromTotalRight, 
    int ToTotalRight, 
    string TestType, 
    string BandScore)
{
    [JsonIgnore]
    public virtual ICollection<TestHistoryDto> TestHistories { get; set; } = new List<TestHistoryDto>();
};