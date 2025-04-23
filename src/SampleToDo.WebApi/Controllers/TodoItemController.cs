using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleToDo.Application.Features.TodoItem.Command.Create;
using SampleToDo.Application.Features.TodoItem.Command.Delete;
using SampleToDo.Application.Features.TodoItem.Command.Update;
using SampleToDo.Application.Features.TodoItem.Query.GetAll;
using SampleToDo.Application.Features.TodoItem.Query.GetById;
using SampleToDo.WebApi.Framework;

namespace SampleToDo.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoItemController(IMediator mediator) : BaseController
{
    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateTodoItemCommand command)
    {
        return OperationResult(await mediator.Send(command));
    }

    [Route("[action]")]
    [HttpPut]
    public async Task<IActionResult> Update(UpdateTodoItemCommand command)
    {
        return OperationResult(await mediator.Send(command));
    }

    [Route("[action]/{id}")]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        return OperationResult(await mediator.Send(new DeleteTodoItemCommand(id)));
    }

    [Route("[action]")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return OperationResult(await mediator.Send(new GetAllTodoItemQuery()));
    }

    [Route("[action]/{id}")]
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        GetByIdTodoItemQuery query = new(id);
        return OperationResult(await mediator.Send(query));
    }
}