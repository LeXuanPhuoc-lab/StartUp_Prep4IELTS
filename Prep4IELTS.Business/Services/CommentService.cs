using System.Linq.Expressions;
using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Business.Services;

public class CommentService(UnitOfWork unitOfWork) : ICommentService
{
    public async Task<IList<CommentDto>> FindAllWithSizeByTestIdAsync(Guid testId, int totalElementSize)
    {
        var commentEntities =  
            await unitOfWork.CommentRepository.FindAllWithSizeByTestIdAsync(testId, totalElementSize);
        return commentEntities.Adapt<List<CommentDto>>();
    }

    public async Task<IList<CommentDto>> FindAllWithConditionAndPagingAsync(
        Expression<Func<Comment, bool>>? filter, 
        Func<IQueryable<Comment>, IOrderedQueryable<Comment>>? orderBy, 
        string? includeProperties, int? pageIndex, int? pageSize)
    {
        var commentEntities =  
            await unitOfWork.CommentRepository.FindAllWithConditionAndPagingAsync(
                filter, orderBy, includeProperties, pageIndex, pageSize);
        return commentEntities.Adapt<List<CommentDto>>();
    }

    public async Task<bool> RemoveRangeCommentAndChildrenAsync(List<CommentDto> comments)
    {
        return await unitOfWork.CommentRepository.RemoveRangeCommentAndChildrenAsync(
            comments.Adapt<List<Comment>>());
    }

    public async Task<int> CountTotalByTestId(Guid testId)
    {
        return await unitOfWork.CommentRepository.CountTotalByTestId(testId); 
    }
}