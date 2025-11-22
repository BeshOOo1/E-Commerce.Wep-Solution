using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Wep.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {

                var errors = actionContext.ModelState.Where(X => X.Value.Errors.Count > 0)
                                                     .ToDictionary(X => X.Key,
                                                     X => X.Value.Errors.Select(X => X.ErrorMessage).ToArray());

                var Problem = new ProblemDetails()
                {
                    Title = "Validation Error",
                    Detail = "One or more validation errors occurred",
                    Status = StatusCodes.Status400BadRequest,
                    Extensions =
                        {
                            {"Errors",errors }
                        }
                };
                return new BadRequestObjectResult(Problem);
           
        }
    }
}
