namespace Prep4IELTS.Data.Dtos;

public record FlashcardDetailTagDto(
    int FlashcardDetailTagId, 
    string FlashcardDetailDesc,
    ICollection<FlashcardDetailDto> FlashcardDetails);