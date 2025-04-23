using MediatR;
using SampleToDo.Application.Models.Common;
using SampleToDo.Domain.Contracts.Persistence;

namespace SampleToDo.Application.Features.TodoItem.Command.Delete;

public class DeleteTodoItemCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTodoItemCommand, OperationResult<bool>>
{
    public async Task<OperationResult<bool>> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItemRepo = unitOfWork.GetRepository<Domain.Entities.TodoItem>();
        var todoItem = await todoItemRepo.FindFirst(q => q.Id == request.Id);
        if (todoItem is null) return OperationResult<bool>.NotFoundResult("Not Found todoItem");
        todoItemRepo.Delete(todoItem);
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}