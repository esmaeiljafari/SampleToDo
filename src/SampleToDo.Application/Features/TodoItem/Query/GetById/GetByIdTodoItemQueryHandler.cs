using AutoMapper;
using MediatR;
using SampleToDo.Application.Features.TodoItem.Shared;
using SampleToDo.Application.Models.Common;
using SampleToDo.Domain.Contracts.Persistence;
using TodoItemEntity = SampleToDo.Domain.Entities.TodoItem;

namespace SampleToDo.Application.Features.TodoItem.Query.GetById;

public class GetByIdTodoItemQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetByIdTodoItemQuery, OperationResult<GetTodoItemQueryResult>>
{
    public async Task<OperationResult<GetTodoItemQueryResult>> Handle(GetByIdTodoItemQuery request,
        CancellationToken cancellationToken)
    {
        var todoItemRepo = unitOfWork.GetRepository<TodoItemEntity>();
        var todoItem = await todoItemRepo.FindFirst(q => q.Id == request.Id);
        if (todoItem is null) return OperationResult<GetTodoItemQueryResult>.NotFoundResult("Not Found todo Item");
        var map = mapper.Map<TodoItemEntity, GetTodoItemQueryResult>(todoItem);
        return OperationResult<GetTodoItemQueryResult>.SuccessResult(map);
    }
}