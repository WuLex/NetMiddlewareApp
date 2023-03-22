using Microsoft.EntityFrameworkCore;
using MiddlewaresDemo.Entities;

namespace MiddlewaresDemo.Middlewares
{
    public class DatabaseMiddleware
    {
        private readonly RequestDelegate _next;

        public DatabaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ChipDbContext dbContext)
        {
            // Perform database operations here
            await dbContext.SaveChangesAsync();

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class DatabaseMiddlewareExtensions
    {
        public static IApplicationBuilder UseDatabaseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DatabaseMiddleware>();
        }
    }
}
