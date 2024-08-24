using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services.Interfaces;

public interface ITagService
{
    IEnumerable<IList<TagDto>> FindTagByTestId(Guid testId);
}