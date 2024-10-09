using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using System.Linq.Expressions;

namespace Prep4IELTS.Business.Services;

public class VocabularyUnitScheduleService(UnitOfWork unitOfWork) : IVocabularyUnitScheduleService
{
    public async Task<bool> InsertAsync(VocabularyUnitScheduleDto vocabularyUnitSchedule)
    {
        await unitOfWork.VocabularyUnitScheduleRepository.InsertAsync(
            vocabularyUnitSchedule.Adapt<VocabularyUnitSchedule>());
        return await unitOfWork.VocabularyUnitScheduleRepository.SaveChangeWithTransactionAsync() > 0;
    }

    public async Task<IList<VocabularyUnitScheduleDto>> FindCalendarAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        var vocabScheduleEntities =
            await unitOfWork.VocabularyUnitScheduleRepository.FindCalendarAsync(userId, startDate, endDate);
        return vocabScheduleEntities.Adapt<List<VocabularyUnitScheduleDto>>();
    }

    public async Task<IList<VocabularyUnitScheduleDto>> FindAllWithConditionAsync(
        Expression<Func<VocabularyUnitSchedule, bool>>? filter,
        Func<IQueryable<VocabularyUnitSchedule>, IOrderedQueryable<VocabularyUnitSchedule>>? orderBy, 
        string? includeProperties)
    {
        var vocabScheduleEntities =
             await unitOfWork.VocabularyUnitScheduleRepository.FindAllWithConditionAsync(
                 filter, orderBy, includeProperties);
        return vocabScheduleEntities.Adapt<List<VocabularyUnitScheduleDto>>();
    }
}