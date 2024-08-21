using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class FlashcardRepository : GenericRepository<Flashcard>
{
    public FlashcardRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }
}