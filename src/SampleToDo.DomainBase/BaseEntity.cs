namespace SampleToDo.DomainBase;

public interface IEntity
{
}

public abstract class BaseEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
    public DateTime? CreatedTime { get; set; }
}

public abstract class BaseEntity : BaseEntity<int>
{
}

public abstract class BaseEntityLong : BaseEntity<long>
{
}