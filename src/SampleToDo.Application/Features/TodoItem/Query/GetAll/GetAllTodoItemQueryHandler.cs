using AutoMapper;
using MediatR;
using SampleToDo.Application.Features.TodoItem.Shared;
using SampleToDo.Application.Models.Common;
using SampleToDo.Domain.Contracts.Persistence;
using TodoItemEntity = SampleToDo.Domain.Entities.TodoItem;

namespace SampleToDo.Application.Features.TodoItem.Query.GetAll;

public class GetAllTodoItemQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllTodoItemQuery, OperationResult<List<GetTodoItemQueryResult>>>
{
    public async Task<OperationResult<List<GetTodoItemQueryResult>>> Handle(GetAllTodoItemQuery request,
        CancellationToken cancellationToken)
    {
        var todoItemRepo = unitOfWork.GetRepository<TodoItemEntity>();
        var todoItem = await todoItemRepo.Query(q => true);
        var map = todoItem.Select(mapper.Map<TodoItemEntity, GetTodoItemQueryResult>).ToList();
        return OperationResult<List<GetTodoItemQueryResult>>.SuccessResult(map);
    }
}