using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockAdvisor.Core.Exceptions;
using StockAdvisor.Infrastructure.Exceptions;

namespace StockAdvisor.Infrastructure.Framework
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                await HandleException(context, e);
            }
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            string errorCode;
            string message;
            HttpStatusCode statusCode;
            var exceptionType = exception.GetType();

            switch(exception)
            {
                case StockAdvisorException e when exception is StockAdvisorException:
                    statusCode = e.CorrespondingStatusCode;
                    errorCode = e.Code;
                    message = e.Message;
                    break;
            
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorCode = "unsupported_error";
                    message = "Report problem to the administration.";
                    break;
            }

            var response = new { code = errorCode, message = message };
            var payload = JsonConvert.SerializeObject(response);
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(payload);
        }
    }
}