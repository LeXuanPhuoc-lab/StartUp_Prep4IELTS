using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Dtos;

namespace Prep4IELTS.Business.Services;

public class TagService(UnitOfWork unitOfWork) : ITagService
{
    
    
    public IEnumerable<IList<TagDto>> FindTagByTestId(Guid testId)
    {
        throw new NotImplementedException();
    }
}