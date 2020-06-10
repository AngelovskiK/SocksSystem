using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BossSystem
{
    public class ExceptionMiddleware { 
        private static readonly Dictionary<Type, HttpStatusCode> statusCodes = new Dictionary<Type, HttpStatusCode>()
        {
                {typeof(NotAuthorizedException), HttpStatusCode.Forbidden},
                {typeof(BadRequestException), HttpStatusCode.BadRequest},
        };

        private readonly RequestDelegate _next;


        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ConfigurateExceptionTypes(exception);
            ExceptionDetails details = new ExceptionDetails
            {
                Message = exception.Message ?? "Internal Server Error." 
            };
            return context.Response.WriteAsync(JsonConvert.SerializeObject(details,
                Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
        private static HttpStatusCode ConfigurateExceptionTypes(Exception exception)
        {
            Type type = exception.GetType();

            // Exception type To Http Status configuration 
            if (statusCodes.ContainsKey(type))
            {
                return statusCodes[type];
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
