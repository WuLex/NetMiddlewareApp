using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace NetCoreDemo
{
    //您可能需要将Microsoft.AspNetCore.Http.Abstractions包安装到项目中
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == 404)
            {
                await httpContext.Response.WriteAsync("I can not find the file.");
            }
            await _next(httpContext);
        }
    }

    //用于将中间件添加到HTTP请求管道的扩展方法。
    public static class Middleware404Extensions
    {
        public static IApplicationBuilder UseNotFoundMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NotFoundMiddleware>();
        }
    }
}
