using System.Linq.Expressions;
using SampleToDo.DomainBase;

namespace SampleToDo.Domain.Contracts.Persistence;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    void AddNew(TEntity entity);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);
    Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> where);
    Task<List<TEntity>> Query(Expression<Func<TEntity, bool>> where);
    void Delete(TEntity entity);
    void Update(TEntity entity);
}