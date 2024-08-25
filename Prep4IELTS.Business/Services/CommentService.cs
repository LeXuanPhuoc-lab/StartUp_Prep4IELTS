using Mapster;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class CommentService(UnitOfWork unitOfWork) : ICommentService
{
    public async Task<IList<CommentDto>> FindAllWithSizeByTestIdAsync(Guid testId, int totalElementSize)
    {
        var commentEntities =  
            await unitOfWork.CommentRepository.FindAllWithSizeByTestIdAsync(testId, totalElementSize);
        return commentEntities.Adapt<List<CommentDto>>();
    }
}