using System.Linq.Expressions;
using Moq;
using SampleToDo.Application.Features.TodoItem.Command.Delete;
using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.Domain.Enums;

namespace UnitTest.Application.Feature.TodoItem.Command;

public class DeleteTodoItemCommandHandlerTests
{
    private DeleteTodoItemCommandHandler _handler;
    private Mock<IRepository<SampleToDo.Domain.Entities.TodoItem>> _mockRepo;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IRepository<SampleToDo.Domain.Entities.TodoItem>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockUnitOfWork.Setup(u => u.GetRepository<SampleToDo.Domain.Entities.TodoItem>())
            .Returns(_mockRepo.Object);

        _mockUnitOfWork.Setup(u => u.CommitAsync())
            .ReturnsAsync(1);

        _handler = new DeleteTodoItemCommandHandler(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task Handle_DeleteExistingTodoItem_ReturnsSuccessResult()
    {
        // Arrange
        var todoItem = new SampleToDo.Domain.Entities.TodoItem
        {
            Id = 1,
            Title = "Sample",
            Description = "Sample desc",
            StatusTodo = StatusTodo.Checked
        };

        _mockRepo.Setup(r => r.FindFirst(It.IsAny<Expression<Func<SampleToDo.Domain.Entities.TodoItem, bool>>>()))
            .ReturnsAsync(todoItem);

        var command = new DeleteTodoItemCommand(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.Delete(todoItem), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task Handle_TodoItemNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        _mockRepo.Setup(r => r.FindFirst(It.IsAny<Expression<Func<SampleToDo.Domain.Entities.TodoItem, bool>>>()))
            .ReturnsAsync((SampleToDo.Domain.Entities.TodoItem)null!);

        var command = new DeleteTodoItemCommand(42);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.Delete(It.IsAny<SampleToDo.Domain.Entities.TodoItem>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Not Found todoItem", result.ErrorMessage);
    }
}