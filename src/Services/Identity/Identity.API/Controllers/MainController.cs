using Identity.API.Models.Errors;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.API.Controllers;

[ApiController]
public abstract class MainController : ControllerBase
{
    private readonly ICollection<string> Errors = new List<string>();

    protected ActionResult CustomResponse(object? result = null)
    {
        if (IsOperationValid())
        {
            return Ok(result);
        }

        return BadRequest(new ErrorData(Errors.ToList()));
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        IEnumerable<ModelError> errors = modelState.Values.SelectMany(e => e.Errors);

        foreach (ModelError? error in errors)
        {
            AddError(error.ErrorMessage);
        }

        return CustomResponse();
    }

    protected bool IsOperationValid()
    {
        return !Errors.Any();
    }

    protected void AddError(string erro)
    {
        Errors.Add(erro);
    }
}
