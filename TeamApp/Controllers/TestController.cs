using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamApp.Models;

namespace TeamApp.Controllers
{
    public class TestController : Controller
    {

        private readonly HttpClient _httpClient;

        public TestController(HttpClient client)
        {
            _httpClient = client;
        }

        public async  Task<IActionResult> Index()
        {
            var result = await _httpClient.GetStringAsync("https://localhost:7267/api/Users/GetUsers");


            var user = JsonConvert.DeserializeObject<IEnumerable<Users>>(result);


            return View(user);
        }
    }
}
