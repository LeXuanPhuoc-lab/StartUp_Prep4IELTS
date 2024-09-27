using System.ComponentModel;

namespace Prep4IELTS.Data.Enum;

public enum FlashcardProgressStatus
{
    [Description(description: "NEW")]
    New,
    [Description(description: "STUDYING")]
    Studying,
    [Description(description: "PROFICIENT")]
    Proficient,
    [Description(description: "STARRED")]
    Starred,
}