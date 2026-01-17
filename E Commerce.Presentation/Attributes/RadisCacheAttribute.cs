using E_Commerce.Services_Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Attributes
{
    public class RadisCacheAttribute:ActionFilterAttribute
    {
        private readonly int _durationTime;
        public RadisCacheAttribute( int durationTime = 5 )
        {
            _durationTime = durationTime;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get CacheService From DI Container
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            // Check IF Data Exisit in Cache
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            var cacheValue = await cacheService.GetAsync(cacheKey);
            // IF Exsist , Return Cached Data And Skip Execution EndPoint
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            // IF Not Exsist ,Execute EndPoint and Store result in Cache IF the Reponse Is 200 OK
             var executedContext = await next.Invoke();
            if(executedContext.Result is OkObjectResult result )
            {
                //await cacheService.SetAsync(cacheKey, result.Value, TimeSpan.FromMinutes(5));
                await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(_durationTime));
            }
        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path);
            foreach(var item in request.Query.OrderBy(X => X.Key))
            {
                key.Append($"|{item.Key}-{item.Value}");
            }
            return key.ToString();
        }
    }
}
