using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.Flashcards;

public class UserFlashcardResponse
{
    public int UserFlashcardId { get; set; }
    public Guid UserId { get; set; }
    public int FlashcardId { get; set; }
    public int? TotalNew { get; set; }
    public int? TotalStudying { get; set; }
    public int? TotalProficient { get; set; }
    public int? TotalStarred { get; set; }
    public UserDto User { get; set; } = null!;
}

