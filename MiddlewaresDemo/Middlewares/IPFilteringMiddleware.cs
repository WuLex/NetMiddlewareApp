using System.Net;

namespace MiddlewaresDemo.Middlewares
{
    /// <summary>
    /// 使用允许的IP地址数组来验证请求是否应该被允许通过。
    /// 如果请求的IP地址不在允许的列表中，
    /// 则中间件返回403 Forbidden状态码并停止请求传递到下一个中间件。
    /// </summary>
    public class IPFilteringMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _allowedIPs;

        public IPFilteringMiddleware(RequestDelegate next, string[] allowedIPs)
        {
            _next = next;
            _allowedIPs = allowedIPs;
        }

        public async Task Invoke(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress;

            if (ipAddress == null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            if (!_allowedIPs.Contains(ipAddress.ToString()))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }

            await _next(context);
        }
    }
}