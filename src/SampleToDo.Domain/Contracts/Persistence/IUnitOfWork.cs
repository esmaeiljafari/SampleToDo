using SampleToDo.DomainBase;

namespace SampleToDo.Domain.Contracts.Persistence;

public interface IUnitOfWork
{
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
    public Task<int> CommitAsync();
    public ValueTask RollBackAsync();
}