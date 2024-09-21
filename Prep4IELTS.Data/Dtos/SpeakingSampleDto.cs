namespace Prep4IELTS.Data.Dtos;

public record SpeakingSampleDto(
    int SpeakingSampleId, 
    string SpeakingSampleName, 
    string? Description, 
    bool IsActive, 
    DateTime CreateDate,
    ICollection<SpeakingPartDto> SpeakingParts,
    ICollection<UserSpeakingSampleHistoryDto> UserSpeakingSampleHistories);