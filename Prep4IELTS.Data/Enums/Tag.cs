using System.ComponentModel;

namespace Prep4IELTS.Data.Enum;

public enum Tag
{
    [Description("IELTS Academic")]
    IeltsAcademic,

    [Description("IELTS General")]
    IeltsGeneral,

    [Description("Reading")]
    Reading,

    [Description("Listening")]
    Listening,

    [Description("Writing")]
    Writing,

    [Description("Speaking")]
    Speaking,
}