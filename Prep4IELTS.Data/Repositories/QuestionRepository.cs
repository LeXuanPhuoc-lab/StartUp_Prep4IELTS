using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Base;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;

namespace Prep4IELTS.Data.Repositories;

public class QuestionRepository : GenericRepository<Question>
{
    public QuestionRepository(Prep4IeltsContext dbContext) 
        : base(dbContext)
    {
    }

    public async Task<Question?> FindQuestionByIdAndWithQuestions(int id)
    {
        return await _dbSet.AsSplitQuery()
            .Where(q => q.QuestionId == id)
            .Include(q => q.QuestionAnswers)
            .FirstOrDefaultAsync();
    }
}