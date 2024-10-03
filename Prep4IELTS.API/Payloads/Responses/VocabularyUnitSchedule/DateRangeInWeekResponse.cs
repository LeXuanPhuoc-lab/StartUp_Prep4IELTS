namespace EXE202_Prep4IELTS.Payloads.Responses.VocabularyUnitSchedule;

public class DateRangeInWeekResponse
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string DateRangeFormat { get; set; } = string.Empty;
}