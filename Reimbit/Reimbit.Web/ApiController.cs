using Common.Security;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Reimbit.Core.Models;

namespace Reimbit.Web;

public class ApiController : ControllerBase
{
    private CurrentUser<TokenData> _currentUser;

    public CurrentUser<TokenData> CurrentUser => _currentUser;

    public ApiController(ICurrentUserProvider currentUserProvider)
    {
        _currentUser = currentUserProvider.GetCurrentUser<TokenData>();
    }

    protected ActionResult Problem(List<Error> errors)
    {
        if (errors.Count == 0)
        {
            return Problem();
        }

        if (errors.All((Error error) => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }

    private ObjectResult Problem(Error error)
    {
        return Problem(null, null, error.Type switch
        {
            ErrorType.Conflict => 409,
            ErrorType.Validation => 400,
            ErrorType.NotFound => 404,
            ErrorType.Unauthorized => 403,
            _ => 500,
        }, error.Description);
    }

    private ActionResult ValidationProblem(List<Error> errors)
    {
        ModelStateDictionary modelStateDictionary = new ModelStateDictionary();

        errors.ForEach(delegate (Error error)
        {
            modelStateDictionary.AddModelError(error.Code, error.Description);
        });

        return ValidationProblem(modelStateDictionary);
    }
}
