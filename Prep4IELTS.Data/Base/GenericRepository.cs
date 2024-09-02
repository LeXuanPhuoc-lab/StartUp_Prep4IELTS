using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Prep4IELTS.Data.Context;

namespace Prep4IELTS.Data.Base;

public class GenericRepository<TEntity> where TEntity : class
{
    private Prep4IeltsContext _dbContext;
    protected DbSet<TEntity> _dbSet;
    
    public GenericRepository(Prep4IeltsContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    protected Prep4IeltsContext DbContext
    {
        get
        {
            if (_dbContext == null!) return new Prep4IeltsContext();
            return _dbContext;
        }
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

    public async Task<TEntity?> FindOneWithConditionAsync(
        Expression<Func<TEntity, bool>>? filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();
        
        if(filter != null)
            query = query.Where(filter);

        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties.Split(
                         new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
                
                // Add AsSplitQuery when includes are present
                query = query.AsSplitQuery();
            }
        }
        
        if (orderBy != null)
            return await orderBy(query).FirstOrDefaultAsync();
        else
            return await query.FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<TEntity>> FindAllWithConditionAsync(
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
                
                // Add AsSplitQuery when includes are present
                query = query.AsSplitQuery();
            }
        }

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        else
            return await query.ToListAsync();
    }

    public async Task<IList<TEntity>> FindAllWithConditionAndThenIncludeAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>? includes = null)
    {
        IQueryable<TEntity> query = _dbSet.AsQueryable();

        if (filter != null)
            query = query.Where(filter);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = include(query);
                
                // Add AsSplitQuery when includes are present
                query = query.AsSplitQuery();
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

        // Set the entity state to Modified
        _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        
        // Get primary key property name 
        var keyProperties = _dbContext.Model.FindEntityType(typeof(TEntity))?.GetProperties();
        
        // Handle identity columns and primary keys
        if (keyProperties != null)
        {
            foreach (var property in keyProperties)
            {
                var propertyEntry = _dbContext.Entry(entityToUpdate).Property(property.Name);

                // Exclude primary key properties from being modified
                if (property.IsKey())
                {
                    propertyEntry.IsModified = false;
                }

                // Exclude identity columns (auto-increment) from being modified
                if (property.ValueGenerated == Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd)
                {
                    propertyEntry.IsModified = false;
                }
            }
        }

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
