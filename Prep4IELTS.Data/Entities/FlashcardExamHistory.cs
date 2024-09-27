using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Entities;

public partial class FlashcardExamHistory
{
    public int FlashcardExamHistoryId { get; set; }

    public int? TotalQuestion { get; set; }
    
    public int? TotalRightAnswer { get; set; }
    
    public int? TotalWrongAnswer { get; set; }
    
    public int? TotalCompletionTime { get; set; }
    
    public double? AccuracyRate { get; set; }
    
    public DateTime TakenDate { get; set; }

    public int UserFlashcardId { get; set; }

    public UserFlashcard UserFlashcard { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<FlashcardExamGrade> FlashcardExamGrades { get; set; } = new List<FlashcardExamGrade>();
}