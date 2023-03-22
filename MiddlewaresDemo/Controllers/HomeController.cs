using Microsoft.AspNetCore.Mvc;
using MiddlewaresDemo.Models;
using MiddlewaresDemo.Utils;
using System.Diagnostics;

namespace MiddlewaresDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
           
            _logger.LogInformation("Index执行");
            return View();
        }

        public IActionResult DBconn()
        {
            //随时获取appsettings.json中的数据
            string connectionString = AppSetting.GetConfig("ConnectionStrings:AdventureWorksDb");
            string connectionStringTwo = AppSetting.GetConfig("AdventureWorksDb");
            var connstr = AppSettingTwo.GetConfig("ConnectionStrings:AdventureWorksDb");
            ViewBag.conn = connstr;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}