using System.Linq.Expressions;
using AutoMapper;
using Moq;
using SampleToDo.Application.Features.TodoItem.Query.GetAll;
using SampleToDo.Application.Features.TodoItem.Shared;
using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.Domain.Enums;

namespace UnitTest.Application.Feature.TodoItem.Query;

public class GetAllTodoItemQueryHandlerTests
{
    private GetAllTodoItemQueryHandler _handler;
    private Mock<IMapper> _mockMapper;
    private Mock<IRepository<SampleToDo.Domain.Entities.TodoItem>> _mockRepo;
    private Mock<IUnitOfWork> _mockUnitOfWork;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IRepository<SampleToDo.Domain.Entities.TodoItem>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();

        _mockUnitOfWork.Setup(u => u.GetRepository<SampleToDo.Domain.Entities.TodoItem>())
            .Returns(_mockRepo.Object);

        _handler = new GetAllTodoItemQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Test]
    public async Task Handle_GetAllTodoItems_ReturnsMappedList()
    {
        // Arrange
        var todoItems = new List<SampleToDo.Domain.Entities.TodoItem>
        {
            new() { Id = 1, Title = "Task 1", Description = "Description 1", StatusTodo = StatusTodo.Checked },
            new() { Id = 2, Title = "Task 2", Description = "Description 2", StatusTodo = StatusTodo.Success }
        };

        var expectedResults = new List<GetTodoItemQueryResult>
        {
            new() { Id = 1, Title = "Task 1", Description = "Description 1", StatusTodo = StatusTodo.Checked },
            new() { Id = 2, Title = "Task 2", Description = "Description 2", StatusTodo = StatusTodo.Success }
        };

        _mockRepo.Setup(r => r.Query(It.IsAny<Expression<Func<SampleToDo.Domain.Entities.TodoItem, bool>>>()))
            .ReturnsAsync(todoItems);

        _mockMapper.Setup(m => m.Map<SampleToDo.Domain.Entities.TodoItem, GetTodoItemQueryResult>(todoItems[0]))
            .Returns(expectedResults[0]);
        _mockMapper.Setup(m => m.Map<SampleToDo.Domain.Entities.TodoItem, GetTodoItemQueryResult>(todoItems[1]))
            .Returns(expectedResults[1]);

        var query = new GetAllTodoItemQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(2, result.Result.Count);
        Assert.AreEqual("Task 1", result.Result[0].Title);
        Assert.AreEqual("Task 2", result.Result[1].Title);
    }
}