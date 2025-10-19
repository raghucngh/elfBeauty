using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace elfBeauty.Middleware.Monitoring
{
    public class PerformanceMonitor
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<PerformanceMonitor> _logger;

        public PerformanceMonitor(RequestDelegate next, ILogger<PerformanceMonitor> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation("Request {Method} {Path} took {ElapsedMilliseconds}ms", context.Request.Method, context.Request.Path, elapsedMilliseconds);
        }
    }
}
