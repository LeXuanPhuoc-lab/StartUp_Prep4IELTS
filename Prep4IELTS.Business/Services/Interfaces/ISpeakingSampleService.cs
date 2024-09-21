using System.Linq.Expressions;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ISpeakingSampleService
{
    Task<SpeakingSampleDto?> FindAsync(int speakingSampleId, Guid userId, int[]? speakingPartIds);
    Task<bool> InsertUserSpeakingSampleHistoryAsync(int speakingSampleId, Guid userId);
    Task<IList<SpeakingSampleDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<SpeakingSample, bool>>? filter,
        Func<IQueryable<SpeakingSample>, IOrderedQueryable<SpeakingSample>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize, Guid userId);

    Task<int> CountTotalAsync();
    Task<bool> IsExistUserSpeakingSampleHistoryAsync(int speakingSampleId, Guid userId);
}