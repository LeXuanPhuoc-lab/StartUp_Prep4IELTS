using System.Linq.Expressions;
using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class SpeakingSampleService(UnitOfWork unitOfWork) : ISpeakingSampleService
{
    public async Task<SpeakingSampleDto?> FindAsync(int speakingSampleId, Guid userId, int[]? speakingPartIds)
    {
        var speakingSampleEntity = await unitOfWork.SpeakingSampleRepository.FindWithUserAsync(speakingSampleId, userId);

        if (speakingSampleEntity == null) return null;
        
        // Get all parts required
        var spkParts = speakingSampleEntity.SpeakingParts.ToList();
        if (speakingPartIds != null && speakingPartIds.Any())
        {
            // Assign speaking parts
            speakingSampleEntity.SpeakingParts = spkParts.Where(sp => 
                speakingPartIds.Contains(sp.SpeakingPartId)).ToList();
        }
        
        return speakingSampleEntity.Adapt<SpeakingSampleDto>();
    }

    public async Task<IList<SpeakingSampleDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<SpeakingSample, bool>>? filter, 
        Func<IQueryable<SpeakingSample>, IOrderedQueryable<SpeakingSample>>? orderBy, 
        string? includeProperties, int? pageIndex,
        int? pageSize, Guid userId)
    {
        var speakingSampleEntities =
            await unitOfWork.SpeakingSampleRepository.FindAllWithConditionAndPagingAsync(filter, orderBy,
                includeProperties, pageIndex, pageSize);

        foreach (var sm in speakingSampleEntities)
        {
            sm.UserSpeakingSampleHistories =
                await unitOfWork.SpeakingSampleRepository.FindUserSpeakingSampleHistoryAsync(sm.SpeakingSampleId,
                    userId);
        }
        
        return speakingSampleEntities.Adapt<List<SpeakingSampleDto>>();
    }

    public async Task<List<SpeakingSampleDto>> FindAllWithConditionAsync(
        Expression<Func<SpeakingSample, bool>> filter, 
        Func<IQueryable<SpeakingSample>, IOrderedQueryable<SpeakingSample>>? orderBy,
        string? includeProperties, Guid userId)
    {
        var speakingSampleEntities =
            await unitOfWork.SpeakingSampleRepository.FindAllWithConditionAsync(filter, orderBy,
                includeProperties);

        foreach (var sm in speakingSampleEntities)
        {
            sm.UserSpeakingSampleHistories =
                await unitOfWork.SpeakingSampleRepository.FindUserSpeakingSampleHistoryAsync(sm.SpeakingSampleId,
                    userId);
        }
        
        return speakingSampleEntities.Adapt<List<SpeakingSampleDto>>();
    }

    public async Task<int> CountTotalAsync()
    {
        return await unitOfWork.SpeakingSampleRepository.CountTotalAsync();
    }

    public async Task<bool> InsertUserSpeakingSampleHistoryAsync(int speakingSampleId, Guid userId)
    {
        return await unitOfWork.SpeakingSampleRepository.InsertUserSpeakingSampleHistoryAsync(
            speakingSampleId, userId);
    }

    public async Task<bool> IsExistUserSpeakingSampleHistoryAsync(int speakingSampleId, Guid userId)
    {
        return await unitOfWork.SpeakingSampleRepository.IsExistUserSpeakingSampleHistoryAsync(speakingSampleId, userId);
    }
}