using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using UPB.BusinessLogic.Managers.Exceptions;

namespace UPB.Practice_2_cert_1.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(PatientAlreadyExistsException ex)
            {
                Log.Warning(ex.Message);
            }
            catch (Exception ex)
            {
                await processException(ex, httpContext);
            }
        }

        private Task processException(Exception ex, HttpContext httpContext)
        {
            Log.Error(ex.Message);
            return httpContext.Response.WriteAsJsonAsync(new { Code = (int)HttpStatusCode.InternalServerError, Error = ex.Message });
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
