using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ICommentService
{
    Task<IList<CommentDto>> FindAllWithSizeByTestIdAsync(Guid testId, int totalElementSize);
}