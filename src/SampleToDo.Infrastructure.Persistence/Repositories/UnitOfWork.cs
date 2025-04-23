using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.DomainBase;

namespace SampleToDo.Infrastructure.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly Dictionary<Type, object?> _repoDictionary = new();

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
    {
        if (_repoDictionary.ContainsKey(typeof(TEntity)))
            return _repoDictionary[typeof(TEntity)] as Repository<TEntity> ?? throw new InvalidOperationException();

        var repository = new Repository<TEntity>(dbContext);
        _repoDictionary.Add(typeof(TEntity), repository);
        return repository;
    }

    public Task<int> CommitAsync()
    {
        return dbContext.SaveChangesAsync();
    }

    public ValueTask RollBackAsync()
    {
        return dbContext.DisposeAsync();
    }
}