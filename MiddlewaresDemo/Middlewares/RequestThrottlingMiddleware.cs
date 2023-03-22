using System.Collections.Concurrent;

namespace MiddlewaresDemo.Middlewares
{
    public class RequestThrottlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, DateTime> _clients = new();
        private readonly int _limit;
        private readonly ILogger<RequestThrottlingMiddleware> _logger;

        public RequestThrottlingMiddleware(RequestDelegate next, int limit, ILogger<RequestThrottlingMiddleware> logger)
        {
            _next = next;
            _limit = limit;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = context.Connection?.RemoteIpAddress?.ToString();
            if (_clients.TryGetValue(clientId ?? "Null", out DateTime lastRequestTime))
            {
                if ((DateTime.UtcNow - lastRequestTime).TotalSeconds < 1)
                {
                    _logger.LogWarning($"Request throttled for {clientId}.");
                    context.Response.StatusCode = 429;
                    return;
                }
            }
            _clients.AddOrUpdate(clientId ?? "Null", DateTime.UtcNow, (_, _) => DateTime.UtcNow);

            await _next(context);
        }
    }

    public static class RequestThrottlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestThrottling(this IApplicationBuilder app, int limit)
        {
            return app.UseMiddleware<RequestThrottlingMiddleware>(limit);
        }
    }
}