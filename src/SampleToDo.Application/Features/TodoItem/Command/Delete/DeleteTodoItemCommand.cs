using MediatR;
using SampleToDo.Application.Models.Common;

namespace SampleToDo.Application.Features.TodoItem.Command.Delete;

public record DeleteTodoItemCommand(int Id) : IRequest<OperationResult<bool>>;