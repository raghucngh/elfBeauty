using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace elfBeauty.Middleware.Logging
{
    public class RequestResponseLog
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLog> _logger;

        public RequestResponseLog(RequestDelegate next, ILogger<RequestResponseLog> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Logs request
            var request = context.Request;
            _logger.LogInformation("Request Information: Method - {Method}, Path - {Path}",
                                    request.Method, request.Path);

            // Logs response
            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(memoryStream).ReadToEnd();
                _logger.LogInformation("Response Information: Status Code: {StatusCode}, Body: {Body}",
                                        context.Response.StatusCode, responseBody);

                memoryStream.Seek(0, SeekOrigin.Begin);

                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}
