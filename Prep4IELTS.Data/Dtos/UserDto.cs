using System.Text.Json.Serialization;

namespace Prep4IELTS.Data.Dtos;

public record UserDto(
    int Id,
    Guid UserId,
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
    int? RoleId, SystemRoleDto Role)
{
    [JsonIgnore] public ICollection<CommentDto> Comments { get; set; } = new List<CommentDto>();
    [JsonIgnore] public ICollection<FlashcardDto> Flashcards { get; set; } = new List<FlashcardDto>();
    [JsonIgnore] public ICollection<TestHistoryDto> TestHistories { get; set; } = new List<TestHistoryDto>();
    [JsonIgnore] public ICollection<TestDto> Tests { get; set; } = new List<TestDto>();
};