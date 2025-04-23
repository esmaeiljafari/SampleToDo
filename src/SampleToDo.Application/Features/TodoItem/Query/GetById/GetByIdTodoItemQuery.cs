using MediatR;
using SampleToDo.Application.Features.TodoItem.Shared;
using SampleToDo.Application.Models.Common;

namespace SampleToDo.Application.Features.TodoItem.Query.GetById;

public record GetByIdTodoItemQuery(int Id) : IRequest<OperationResult<GetTodoItemQueryResult>>;