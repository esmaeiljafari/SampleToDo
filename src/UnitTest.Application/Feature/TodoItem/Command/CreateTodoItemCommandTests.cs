using Moq;
using SampleToDo.Application.Features.TodoItem.Command.Create;
using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.Domain.Enums;

namespace UnitTest.Application.Feature.TodoItem.Command;

public class CreateTodoItemCommandTests
{
    [Test]
    public async Task Handle_CreateTodoItemCommand_ReturnsSuccessResultWithId()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<SampleToDo.Domain.Entities.TodoItem>>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockUnitOfWork.Setup(u => u.GetRepository<SampleToDo.Domain.Entities.TodoItem>())
            .Returns(mockRepo.Object);
        mockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        var command = new CreateTodoItemCommand
        {
            Title = "Test Title",
            Description = "Test Description",
            StatusTodo = StatusTodo.Success
        };

        var handler = new CreateTodoItemCommandHandler(mockUnitOfWork.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.IsSuccess);
    }
}