namespace Prep4IELTS.Data.Entities;

public partial class FlashcardDetailTag
{
    public int FlashcardDetailTagId { get; set; }
    public string FlashcardDetailDesc { get; set; } = null!;
    
    public virtual ICollection<FlashcardDetail> FlashcardDetails { get; set; } = new List<FlashcardDetail>();
}