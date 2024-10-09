using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using System.Linq.Expressions;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface IVocabularyUnitScheduleService
{
    Task<bool> InsertAsync(VocabularyUnitScheduleDto vocabularyUnitSchedule);
    Task<IList<VocabularyUnitScheduleDto>> FindCalendarAsync(Guid userId, DateTime startDate, DateTime endDate);
    Task<IList<VocabularyUnitScheduleDto>> FindAllWithConditionAsync(
        Expression<Func<VocabularyUnitSchedule, bool>>? filter,
        Func<IQueryable<VocabularyUnitSchedule>, IOrderedQueryable<VocabularyUnitSchedule>>? orderBy,
        string? includeProperties);
}