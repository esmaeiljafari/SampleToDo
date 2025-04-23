using MediatR;
using SampleToDo.Application.Models.Common;
using SampleToDo.Domain.Contracts.Persistence;

namespace SampleToDo.Application.Features.TodoItem.Command.Update;

public class UpdateTodoItemCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTodoItemCommand, OperationResult<bool>>
{
    public async Task<OperationResult<bool>> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItemRepo = unitOfWork.GetRepository<Domain.Entities.TodoItem>();
        var todoItem = await todoItemRepo.FindFirst(q => q.Id == request.Id);
        if (todoItem is null) return OperationResult<bool>.NotFoundResult("Not Found todoItem");

        todoItem.StatusTodo = request.StatusTodo;
        todoItem.Title = request.Title;
        todoItem.Description = request.Description;
        todoItemRepo.Update(todoItem);
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}