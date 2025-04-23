using FluentValidation;
using MediatR;
using SampleToDo.Application.Models.Common;
using SampleToDo.Domain.Enums;

namespace SampleToDo.Application.Features.TodoItem.Command.Create;

public class CreateTodoItemCommand : IRequest<OperationResult<int>>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public StatusTodo StatusTodo { get; set; }
}

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotNull().WithMessage("Title must not be Null.")
            .NotEmpty().WithMessage("Title must not be empty.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters.");

        RuleFor(c => c.Description)
            .NotNull().WithMessage("Description must not be Null.")
            .NotEmpty().WithMessage("Description must not be empty.");

        RuleFor(x => x.StatusTodo)
            .Must(value => Enum.IsDefined(typeof(StatusTodo), value))
            .WithMessage("Invalid status value.");
    }
}