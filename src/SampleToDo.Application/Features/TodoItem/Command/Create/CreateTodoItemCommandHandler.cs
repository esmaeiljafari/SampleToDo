using MediatR;
using SampleToDo.Application.Models.Common;
using SampleToDo.Domain.Contracts.Persistence;

namespace SampleToDo.Application.Features.TodoItem.Command.Create;

public class CreateTodoItemCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTodoItemCommand, OperationResult<int>>
{
    public async Task<OperationResult<int>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItemRepo = unitOfWork.GetRepository<Domain.Entities.TodoItem>();
        var todoItem = new Domain.Entities.TodoItem
        {
            CreatedTime = DateTime.Now,
            Description = request.Description,
            StatusTodo = request.StatusTodo,
            Title = request.Title
        };
        todoItemRepo.AddNew(todoItem);

        await unitOfWork.CommitAsync();
        return OperationResult<int>.SuccessResult(todoItem.Id);
    }
}