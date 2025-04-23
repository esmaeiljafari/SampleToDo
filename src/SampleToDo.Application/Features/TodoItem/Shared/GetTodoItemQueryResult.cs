using System.Text.Json.Serialization;
using SampleToDo.Application.Profiles;
using SampleToDo.Domain.Enums;

namespace SampleToDo.Application.Features.TodoItem.Shared;

public class GetTodoItemQueryResult : ICreateMapper<Domain.Entities.TodoItem>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusTodo StatusTodo { get; set; }
}