using Microsoft.AspNetCore.Mvc;
using SampleToDo.Application.Models.Common;

namespace SampleToDo.WebApi.Framework;

public class BaseController : ControllerBase
{
    protected IActionResult OperationResult<TModel>(OperationResult<TModel> result)
    {
        if (result.IsSuccess) return result.Result is bool ? Ok() : Ok(result.Result);

        if (result.IsNotFound)
        {
            ModelState.AddModelError("GeneralError", result.ErrorMessage);
            var notFoundErrors = new ValidationProblemDetails(ModelState);
            return NotFound(notFoundErrors.Errors);
        }

        switch (result.CustomCode)
        {
            case > 0:
                ModelState.AddModelError("GatewayError", result.ErrorMessage);
                return StatusCode(result.CustomCode);
        }

        return BadRequest(result.ErrorMessage);
    }
}