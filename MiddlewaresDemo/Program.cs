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
                //�� lambda ȷ�����������Ƿ���Ҫ�û�ͬ��Ǳ�Ҫ cookie��
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            // ��ȡ���ݿ������ַ���
            var Configuration = builder.Configuration;
          
            // ��� DbContext ����
            builder.Services.AddDbContext<ChipDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddMemoryCache();

            //�Զ�����ͼ���������ͼ�ķ�ʽ
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

            // ���Serilog��־��¼
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

            // ע��LoggingMiddleware
            app.UseMiddleware<LoggingMiddleware>();
            app.UseNotFoundMiddleware(); /*ʹ���м��*/
            //���ݿ��м����ӵ�HTTP�ܵ���
            app.UseDatabaseMiddleware();
            app.UseCacheMiddleware();
            //������������Ϊ10��������ζ��ÿ���ͻ�����һ���������ֻ�ܷ���10��
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