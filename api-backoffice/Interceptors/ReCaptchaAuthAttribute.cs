using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace api_public_backOffice.Interceptors
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ReCaptchaAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string TokenHeaderName = "tokenRC";
        private const string SecretReCaptcha = "SecretReCaptcha";
        private const string UrlReCaptcha = "UrlReCaptcha";
        private double THRESHOLD;
        private string Action;

        public ReCaptchaAuthAttribute(string actions, double threashold = 0.5)
        {
            Action = actions;
            THRESHOLD = threashold;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(TokenHeaderName, out var requestTokenRC))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var configuracion = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            string secret = configuracion.GetValue<string>(SecretReCaptcha);
            string url = configuracion.GetValue<string>(UrlReCaptcha);

            if (string.IsNullOrEmpty(secret)
                || string.IsNullOrEmpty(url))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var client = new RestClient($"{ url }") { Timeout = -1 };
            var request = new RestRequest(Method.POST);
            request.AddParameter("secret", secret, ParameterType.QueryString);
            request.AddParameter("response", requestTokenRC, ParameterType.QueryString);

            IRestResponse response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var ResponseReCaptchaVerify = JsonConvert.DeserializeObject<ResponseReCaptchaVerify>(response.Content);

            //Control de respuesta reCaptcha
            if (!ResponseReCaptchaVerify.Success ||
                ResponseReCaptchaVerify.Score < THRESHOLD ||
                !ResponseReCaptchaVerify.Action.Equals(Action))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }

        private class ResponseReCaptchaVerify
        {
            public bool Success { get; set; } // whether this request was a valid reCAPTCHA token for your site
            public double Score { get; set; } // the score for this request (0.0 - 1.0)
            public string Hostname { get; set; } // the hostname of the site where the reCAPTCHA was solved
            public string Action { get; set; } // the action name for this request (important to verify)
            public string[] ErrorCode { get; set; } // optional
            public DateTime challenge_ts { get; set; }

        }
    }
}
