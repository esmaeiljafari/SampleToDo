using System.Linq.Expressions;
using AutoMapper;
using Moq;
using SampleToDo.Application.Features.TodoItem.Query.GetById;
using SampleToDo.Application.Features.TodoItem.Shared;
using SampleToDo.Domain.Contracts.Persistence;
using SampleToDo.Domain.Enums;

namespace UnitTest.Application.Feature.TodoItem.Query;

public class GetByIdTodoItemQueryHandlerTests
{
    private GetByIdTodoItemQueryHandler _handler;
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

        _handler = new GetByIdTodoItemQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object);
    }

    [Test]
    public async Task Handle_GetById_ReturnsMappedResult_WhenFound()
    {
        // Arrange
        var todoEntity = new SampleToDo.Domain.Entities.TodoItem
        {
            Id = 5,
            Title = "Test Todo",
            Description = "Test Description",
            StatusTodo = StatusTodo.UnderReview
        };

        var expectedDto = new GetTodoItemQueryResult
        {
            Id = 5,
            Title = "Test Todo",
            Description = "Test Description",
            StatusTodo = StatusTodo.UnderReview
        };

        _mockRepo.Setup(r => r.FindFirst(It.IsAny<Expression<Func<SampleToDo.Domain.Entities.TodoItem, bool>>>()))
            .ReturnsAsync(todoEntity);

        _mockMapper.Setup(m => m.Map<SampleToDo.Domain.Entities.TodoItem, GetTodoItemQueryResult>(todoEntity))
            .Returns(expectedDto);

        var query = new GetByIdTodoItemQuery(5);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(expectedDto.Id, result.Result.Id);
        Assert.AreEqual(expectedDto.Title, result.Result.Title);
    }

    [Test]
    public async Task Handle_GetById_ReturnsNotFound_WhenEntityIsNull()
    {
        // Arrange
        _mockRepo.Setup(r => r.FindFirst(q => q.Id == 99))
            .ReturnsAsync((SampleToDo.Domain.Entities.TodoItem)null!);

        var query = new GetByIdTodoItemQuery(99);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Not Found todo Item", result.ErrorMessage);
        Assert.IsNull(result.Result);
    }
}