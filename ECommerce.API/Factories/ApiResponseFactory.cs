using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Factories
{
    public static class ApiResponseFactory
    {
        public static ActionResult GenerateApiValidationResponse(ActionContext actionContext)
        {
            var error = actionContext.ModelState.Where(X => X.Value.Errors.Count > 0)
                .ToDictionary(
                X => X.Key,
                X => X.Value.Errors.Select(X => X.ErrorMessage).ToArray()
                );
            var Problem = new ProblemDetails()
            {
                Title = "Validation Errors",
                Detail = "One Or More Valiadtion Errors Occurred",
                Status = StatusCodes.Status400BadRequest,
                Extensions = { { "Error", error } }
            };
            return new BadRequestObjectResult(Problem);
        }
    }
}
