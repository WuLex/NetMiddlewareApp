using Serilog;
using ILogger = Serilog.ILogger;

namespace MiddlewaresDemo.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = Log.ForContext<LoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            // 记录请求的详细信息
            _logger.Information("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

            // 缓存响应流，以便记录响应的详细信息
            var responseBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                // 记录响应的详细信息
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(memoryStream).ReadToEnd();
                _logger.Information("Outgoing response: {StatusCode} {Body}", context.Response.StatusCode, responseBody);

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(responseBodyStream);
            }
        }
    }
}