using E_Commerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.CustomMiddleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next,ILogger<ExceptionHandlerMiddleware>logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
                if(httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    var Problem = new ProblemDetails()
                    {
                        Title = "Error While Processing Http - End Point Not Found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = $"EndPoint {httpContext.Request.Path} Not Found",
                        Instance = httpContext.Request.Path
                    };
                    await httpContext.Response.WriteAsJsonAsync(Problem);
                }
            }
            catch (Exception ex)
            {

                // Loging the exception can be done here
                _logger.LogError(ex, "An Unexpected Error Occured");
               // httpContext.Response.StatusCode =StatusCodes.Status500InternalServerError;
                // Custom Error Response 
                var Problem = new ProblemDetails()
                {
                    Title = "An unexpected error occurred!",
                    Detail = ex.Message,
                    Instance = httpContext.Request.Path,
                    Status = ex switch
                    {
                        NotFoundException=>StatusCodes.Status404NotFound,
                        _=> StatusCodes.Status500InternalServerError
                    }
                  
                };
                httpContext.Response.StatusCode = Problem.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
