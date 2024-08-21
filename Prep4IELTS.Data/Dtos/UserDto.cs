namespace Prep4IELTS.Data.Dtos;

public record UserDto(int Id, Guid UserId, 
    string ClerkId, 
    string FirstName, 
    string LastName, 
    string Email, 
    string? Phone, 
    string? AvatarImage, 
    bool? IsActive, 
    DateTime? DateOfBirth,
    DateTime? CreateDate, 
    DateTime? TestTakenDate, 
    string? TargetScore, 
    int? RoleId, SystemRoleDto Role,
    ICollection<CommentDto> Comments,
    ICollection<FlashcardDto> Flashcards,
    ICollection<TestHistoryDto> TestHistories);