using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.DomainBase;

namespace SampleToDo.Infrastructure.Persistence.Repositories;

public class Repository<TEntity>(ApplicationDbContext dbContext) : IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public void AddNew(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where)
    {
        return await _dbSet.AnyAsync(where);
    }

    public async Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> where)
    {
        return await _dbSet.FirstOrDefaultAsync(where);
    }

    public async Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where)
    {
        return await _dbSet.AsQueryable().Where(where).ToListAsync();
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
}