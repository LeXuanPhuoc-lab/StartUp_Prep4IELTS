using Prep4IELTS.Business.Constants;
using Prep4IELTS.Data.Dtos;

namespace EXE202_Prep4IELTS.Payloads.Responses.VocabularyUnitSchedule;

public class VocabularyCalendarResponse
{
    public List<VocabularyUnitScheduleDto> VocabInMon { get; set; } = new();
    public List<VocabularyUnitScheduleDto> VocabInTues { get; set; } = new();
    public List<VocabularyUnitScheduleDto> VocabInWed { get; set; } = new();
    public List<VocabularyUnitScheduleDto> VocabInThurs { get; set; } = new();
    public List<VocabularyUnitScheduleDto> VocabInFri { get; set; } = new();
    public List<VocabularyUnitScheduleDto> VocabInSat { get; set; } = new();
    public List<VocabularyUnitScheduleDto> VocabInSun { get; set; } = new();
}

public static class VocabularyCalendarResponseExtensions
{
    public static VocabularyCalendarResponse ToVocabularyCalendarResponse(
        this IList<VocabularyUnitScheduleDto> vocabularyScheduleDtos)
    {
        if(!vocabularyScheduleDtos.Any()) return new VocabularyCalendarResponse();
        
        var vocabScheduleByWeekday = vocabularyScheduleDtos
            .GroupBy(v => v.Weekday)
            .ToList();

        // Initiate vocab calendar resp
        var vocabCalendarResp = new VocabularyCalendarResponse();
        foreach (var vScheduleByWeek in vocabScheduleByWeekday)
        {
            switch (vScheduleByWeek.Key)
            {
                case WeekDayConstants.Monday:
                    vocabCalendarResp.VocabInMon.AddRange(vScheduleByWeek);
                    break;
                case WeekDayConstants.Tuesday:
                    vocabCalendarResp.VocabInTues.AddRange(vScheduleByWeek);
                    break;
                case WeekDayConstants.Wednesday:
                    vocabCalendarResp.VocabInWed.AddRange(vScheduleByWeek);
                    break;
                case WeekDayConstants.Thursday:
                    vocabCalendarResp.VocabInThurs.AddRange(vScheduleByWeek);
                    break;
                case WeekDayConstants.Friday:
                    vocabCalendarResp.VocabInFri.AddRange(vScheduleByWeek);
                    break;
                case WeekDayConstants.Saturday:
                    vocabCalendarResp.VocabInSat.AddRange(vScheduleByWeek);
                    break;
                case WeekDayConstants.Sunday:
                    vocabCalendarResp.VocabInSun.AddRange(vScheduleByWeek);
                    break;
            }
        }
        
        return vocabCalendarResp;
    }
}