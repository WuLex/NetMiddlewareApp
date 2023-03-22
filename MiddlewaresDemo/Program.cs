using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiddlewaresDemo.Entities;
using MiddlewaresDemo.Implements;
using MiddlewaresDemo.Middlewares;
using NetCoreDemo;
using Serilog;
using Serilog.Events;
using System;

namespace MiddlewaresDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {


         
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                //此 lambda 确定给定请求是否需要用户同意非必要 cookie。
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            // 获取数据库连接字符串
            var Configuration = builder.Configuration;
          
            // 添加 DbContext 服务
            builder.Services.AddDbContext<ChipDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddMemoryCache();

            //自定义视图引擎查找视图的方式
            builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
            });

            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                         .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                        //.WriteTo.Console()
                        .CreateLogger();
            //Log.Logger = new LoggerConfiguration()
            //            .MinimumLevel.Information()
            //            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            //            .Enrich.FromLogContext()
            //            .WriteTo.Console()
            //            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            //            .CreateLogger();

            // 添加Serilog日志记录
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });
            //builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
           builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // 注册LoggingMiddleware
            app.UseMiddleware<LoggingMiddleware>();
            app.UseNotFoundMiddleware(); /*使用中间件*/
            //数据库中间件添加到HTTP管道中
            app.UseDatabaseMiddleware();
            app.UseCacheMiddleware();
            //请求限制设置为10个，这意味着每个客户端在一秒钟内最多只能发送10个
            //app.UseRequestThrottling(10);
            //app.UseMiddleware<IPFilteringMiddleware>(new string[] { "127.0.0.1", "::1" });

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}