using E_Commerce.Shared.CommonResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ApiBaseController : ControllerBase
    {
        // Handle Result Without Value
        // if Result is Succuss Return NoContent 204
        // if Result is Failure Return Problem with Status Code and Error Details
        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProblem(result.Errors);
        }


        // Handle Result Without Value
        // if Result is Succuss Return Ok 200 With Value
        // if Result is Failure Return Problem with Status Code and Error Details
        protected ActionResult<TValue> HandleResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return HandleProblem(result.Errors);
        }
        private ActionResult HandleProblem(IReadOnlyList<Error> errors)
        {
            // if No Error are Provided , Return 500 Error
            if (errors.Count == 0)
                return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An UnExpected Error Occured");
            
            // if there is only one Error , Handle it as a Single Error Problem
            if(errors.All(e => e.ErrorType == ErrorType.Validation))
                return HandleValidationProblem(errors);
            return HandleSingleErrorProblem(errors[0]);

            // if All Errors are Validation Errors, Handle the as Validation Problem

        }
        private ActionResult HandleSingleErrorProblem(Error error)
        {
            return Problem(
                title: error.Code,
                detail: error.Description,
                type: error.ErrorType.ToString(),
                statusCode: MapErrorTypeToStatusCode(error.ErrorType)
                );
        }
        private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthrized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.InvalidCrendentials => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        private ActionResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var ModelState = new ModelStateDictionary();
            foreach (var error in errors)
                ModelState.AddModelError(error.Code, error.Description);

            return ValidationProblem(ModelState);
        }
    }
}
