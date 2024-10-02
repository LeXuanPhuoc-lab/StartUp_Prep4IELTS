using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class VocabularyUnitScheduleService(UnitOfWork unitOfWork) : IVocabularyUnitScheduleService
{
    public async Task<bool> InsertAsync(VocabularyUnitScheduleDto vocabularyUnitSchedule)
    {
        await unitOfWork.VocabularyUnitScheduleRepository.InsertAsync(
            vocabularyUnitSchedule.Adapt<VocabularyUnitSchedule>());
        return await unitOfWork.VocabularyUnitScheduleRepository.SaveChangeWithTransactionAsync() > 0;
    }
}