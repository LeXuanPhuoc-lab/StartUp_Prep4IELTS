using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IVocabularyUnitScheduleService
{
    Task<bool> InsertAsync(VocabularyUnitScheduleDto vocabularyUnitSchedule);
}