using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Polly;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MiddlewaresDemo.Middlewares
{
    public class PollyMiddleware
    {
        private readonly RequestDelegate _next;

        public PollyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var policy = Policy
                 .Handle<TimeoutException>()
                 .Or<WebException>()
                 .Or<HttpRequestException>()
                 .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                 .WrapAsync(Policy.TimeoutAsync<HttpResponseMessage>(10));

            var response = await policy.ExecuteAsync(() => (Task<HttpResponseMessage>)_next(context));
            await response.Content.ReadAsStringAsync();
            await context.Response.WriteAsync("Response from PollyMiddleware: " + await response.Content.ReadAsStringAsync());
        }
    }

    public static class PollyMiddlewareExtensions
    {
        public static IApplicationBuilder UsePollyMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<PollyMiddleware>();
        }
    }
}
