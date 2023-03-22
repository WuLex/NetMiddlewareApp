using Microsoft.Extensions.Caching.Memory;

namespace MiddlewaresDemo.Middlewares
{
    public class CacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;

        public CacheMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _cache = memoryCache;
        }

        public async Task Invoke(HttpContext context)
        {
            // Generate a cache key based on the request URL
            var cacheKey = context.Request.Path.ToString();

            // Attempt to retrieve the cached response
            if (_cache.TryGetValue(cacheKey, out byte[] cachedResponse))
            {
                // If the cached response exists, return it directly to the client
                context.Response.StatusCode = 200;
                context.Response.ContentType = "text/plain";
                await context.Response.Body.WriteAsync(cachedResponse);
            }
            else
            {
                // If the cached response doesn't exist, continue down the pipeline
                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);

                    // If the response is successful, cache the response
                    if (context.Response.StatusCode == 200)
                    {
                        // Set the cache expiration to 30 seconds
                        var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
                        cachedResponse = responseBody.ToArray();
                        _cache.Set(cacheKey, cachedResponse, cacheOptions);
                        await originalBodyStream.WriteAsync(cachedResponse);
                    }
                    else
                    {
                        // If the response is not successful, don't cache the response
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                }
            }
        }
    }

    public static class CacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseCacheMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CacheMiddleware>();
            return app;
        }
    }
}
