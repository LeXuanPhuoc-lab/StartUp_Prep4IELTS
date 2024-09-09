using System.Linq.Expressions;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ICommentService
{
    Task<IList<CommentDto>> FindAllWithSizeByTestIdAsync(Guid testId, int totalElementSize);
    Task<IList<CommentDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Comment, bool>>? filter,
        Func<IQueryable<Comment>, IOrderedQueryable<Comment>>? orderBy,
        string? includeProperties,
        int? pageIndex, int? pageSize);

    Task<bool> RemoveRangeCommentAndChildrenAsync(List<CommentDto> comments);   
    Task<int> CountTotalByTestId(Guid testId);
}