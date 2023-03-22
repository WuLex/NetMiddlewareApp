using Microsoft.AspNetCore.Mvc;

namespace MiddlewaresDemo.Controllers
{
    public class MidController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.baidu.com");
            var response = await httpClient.SendAsync(request);
            return Ok(response);
        }
    }
}
