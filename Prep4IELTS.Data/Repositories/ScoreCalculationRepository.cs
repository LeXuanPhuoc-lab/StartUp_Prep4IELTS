using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class ScoreCalculationRepository : GenericRepository<ScoreCalculation>
{
    public ScoreCalculationRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }
    
    public async Task<ScoreCalculation?> GetByTotalRightAnswerAndTestType(int totalRightAnswer, string testType)
    {
        return await _dbSet.FirstOrDefaultAsync(sc =>
            sc.FromTotalRight <= totalRightAnswer && sc.ToTotalRight >= totalRightAnswer &&
            sc.TestType.Equals(testType));
    }
}