using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class PaymentTypeRepository : GenericRepository<PaymentType>
{
    public PaymentTypeRepository(Prep4IeltsContext dbContext) : base(dbContext)
    {
    }
    
    
}