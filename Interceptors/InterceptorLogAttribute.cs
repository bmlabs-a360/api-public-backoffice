using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace api_public_backOffice.Interceptors
{
    public class InterceptorLogAttribute : ActionFilterAttribute
    {

        private readonly ILogger _logger;

        public InterceptorLogAttribute(ILogger<InterceptorLogAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var entries = actionContext.ActionArguments.Select(d => string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value?.ToString())));
            var parameters= "{" + string.Join(",", entries) + "}";
            _logger.LogInformation("-------> IN {0}, parameters: {1}", actionContext.ActionDescriptor.DisplayName, parameters);
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(ActionExecutedContext actionContext)
        {
            _logger.LogInformation("<------- OUT {0}", actionContext.ActionDescriptor.DisplayName);
            base.OnActionExecuted(actionContext);
        }
    }
}