using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace api_public_backOffice.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";
        private const string ApiKeyConfigName = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potencialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var configuracion = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuracion.GetValue<string>(ApiKeyConfigName);

            if (!apiKey.Equals(potencialApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            await next();
        }
    }
}
