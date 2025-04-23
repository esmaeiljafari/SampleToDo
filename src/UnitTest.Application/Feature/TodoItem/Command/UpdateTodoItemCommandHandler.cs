using System.Linq.Expressions;
using Moq;
using SampleToDo.Application.Features.TodoItem.Command.Update;
using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.Domain.Enums;

namespace UnitTest.Application.Feature.TodoItem.Command;

public class UpdateTodoItemCommandHandlerTests
{
    private UpdateTodoItemCommandHandler _handler;
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

        _handler = new UpdateTodoItemCommandHandler(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task Handle_UpdateExistingTodoItem_ReturnsSuccessResult()
    {
        // Arrange
        var existingItem = new SampleToDo.Domain.Entities.TodoItem
        {
            Id = 1,
            Title = "Old Title",
            Description = "Old Desc",
            StatusTodo = StatusTodo.Success
        };

        var command = new UpdateTodoItemCommand
        {
            Id = 1,
            Title = "Updated Title",
            Description = "Updated Desc",
            StatusTodo = StatusTodo.Checked
        };

        _mockRepo.Setup(r => r.FindFirst(It.IsAny<Expression<Func<SampleToDo.Domain.Entities.TodoItem, bool>>>()))
            .ReturnsAsync(existingItem);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.Update(It.Is<SampleToDo.Domain.Entities.TodoItem>(
            t => t.Id == command.Id &&
                 t.Title == command.Title &&
                 t.Description == command.Description &&
                 t.StatusTodo == command.StatusTodo)), Times.Once);

        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task Handle_TodoItemNotFound_ReturnsNotFoundResult()
    {
        // Arrange
        var command = new UpdateTodoItemCommand
        {
            Id = 42,
            Title = "Doesn't matter",
            Description = "Not found case",
            StatusTodo = StatusTodo.UnderReview
        };

        _mockRepo.Setup(r => r.FindFirst(It.IsAny<Expression<Func<SampleToDo.Domain.Entities.TodoItem, bool>>>()))
            .ReturnsAsync((SampleToDo.Domain.Entities.TodoItem)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.Update(It.IsAny<SampleToDo.Domain.Entities.TodoItem>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.CommitAsync(), Times.Never);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Not Found todoItem", result.ErrorMessage);
    }
}