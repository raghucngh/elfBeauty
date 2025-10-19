using elfBeauty.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace elfBeauty.Middleware.Logging
{
    public class ErrorHandlingLog
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingLog> _logger;

        public ErrorHandlingLog(RequestDelegate next, ILogger<ErrorHandlingLog> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Calls next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Logs exception
                _logger.LogError($"{Const.ErrorLog_Simple}: {ex.Message}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = Const.ContentType_Json;
                await context.Response.WriteAsync(Const.ErrorLog_Unexpected);
            }
        }
    }
}
