using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;

namespace Prep4IELTS.Data.Repositories;

public class TestSectionRepository : GenericRepository<TestSectionRepository>
{
    public TestSectionRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }
}