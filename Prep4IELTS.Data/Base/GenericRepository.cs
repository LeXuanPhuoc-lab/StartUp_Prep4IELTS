using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Context;

namespace Prep4IELTS.Data.Base;

public class GenericRepository<TEntity> where TEntity : class
{
    private Prep4IeltsContext _dbContext;
    private DbSet<TEntity> _dbSet;
    
    public GenericRepository(Prep4IeltsContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }
    
    #region Retrieve operation
    public TEntity? Find(params object[] keyValues)
    {
        return _dbSet.Find(keyValues);
    }
    
    public IEnumerable<TEntity> FindAll()
    {
        return _dbSet.ToList();
    }
    
    public async Task<TEntity?> FindAsync(params object[] keyValues)
    {
        return await _dbSet.FindAsync(keyValues);
    }

    public async Task<IEnumerable<TEntity>> FindAllAsync()
    {
        return await _dbSet.ToListAsync();
    }
    
    public IEnumerable<TEntity> FindWithCondition(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(
                         new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }
    
    public async Task<IEnumerable<TEntity>> FindWithConditionAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(
                         new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        else
            return await query.ToListAsync();
    }
    #endregion

    #region Insert/Update/Remove operation
    public void Insert(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }
    
    public void Remove(object id)
    {
        var entityToDelete = _dbSet.Find(id);
        if(entityToDelete != null) PerformRemove(entityToDelete);
    }
    public async Task RemoveAsync(object id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        if(entityToDelete != null) PerformRemove(entityToDelete);
    }
    private void PerformRemove(TEntity entityToDelete)
    {
        if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }

    public async Task UpdateAsync(TEntity entityToUpdate, bool saveChanges = false)
    {
        _dbSet.Attach(entityToUpdate);
        _dbContext.Entry(entityToUpdate).State = EntityState.Modified;

        if (saveChanges) await SaveChangeAsync();
    }
    
    #endregion
    
    #region Save operation
    public async Task<int> SaveChangeAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
    
    public int SaveChangeWithTransaction()
    {
        int result = -1;

        using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                result = _dbContext.SaveChanges();
                dbContextTransaction.Commit();  
            }
            catch (Exception)
            {
                result = -1;
                _dbContext.Database.RollbackTransaction();
            }
        }

        return result;
    }
    
    public async Task<int> SaveChangeWithTransactionAsync()
    {
        int result = -1;

        using (var dbContextTransaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                result = await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();  
            }
            catch (Exception)
            {
                result = -1;
                await _dbContext.Database.RollbackTransactionAsync();
            }
        }

        return result;
    }
    #endregion
}
