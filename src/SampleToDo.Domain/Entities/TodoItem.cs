using SampleToDo.Domain.Enums;
using SampleToDo.DomainBase;

namespace SampleToDo.Domain.Entities;

public class TodoItem : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public StatusTodo StatusTodo { get; set; }
}