using E_Commerce.Shared.CommonResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiBaseController:ControllerBase
    {
        
        
        protected IActionResult HandleResult (Result result)
        {
            if (result.IsSuccess)
                return NoContent();
            else
                return HandleProplem(result.Errors);
        }



        protected ActionResult<TValue>HandelResult<TValue>(Result<TValue> result)
        {
            if (result.IsSuccess)
                return Ok(result.Value);
            else
                return HandleProplem(result.Errors);
        }

        private ActionResult HandleProplem(IReadOnlyList<Error> errors)
        {
            // IF Error But Don't Have Any Return 
            if(errors.Count == 0)
            {
                return Problem
                    (
                    title: "An Error  Occurred",
                    statusCode: StatusCodes.Status500InternalServerError
                    );
            }

            // IF Validation Errors 

            if(errors.All(E=>E.ErrorType==ErrorTypes.Validation))
                return HandleValidationErrors(errors);

            // IF Error With SingleError

                return HandleSingleProblem(errors[0]);
        }

        private ActionResult HandleSingleProblem(Error error)
           =>  Problem
               (
                title: error.Code,
                detail: error.Description,
                type: error.ErrorType.ToString(),
                statusCode: MapErrorTypeIntoStatusCode(error.ErrorType)
                );
        
        private ActionResult HandleValidationErrors(IReadOnlyList<Error> errors)
        {
            var modelState = new ModelStateDictionary();
            foreach(var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(modelState);
        }


        private static int MapErrorTypeIntoStatusCode(ErrorTypes errorType)
        {
            return errorType switch
            {
                ErrorTypes.Validation => StatusCodes.Status400BadRequest,
                ErrorTypes.UnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorTypes.Forbidden => StatusCodes.Status403Forbidden,
                ErrorTypes.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        }
         
    }
}
