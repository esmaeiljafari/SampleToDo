using MediatR;
using SampleToDo.Application.Features.TodoItem.Shared;
using SampleToDo.Application.Models.Common;

namespace SampleToDo.Application.Features.TodoItem.Query.GetAll;

public class GetAllTodoItemQuery : IRequest<OperationResult<List<GetTodoItemQueryResult>>>;