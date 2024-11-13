using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.Json;
using TeamApp.Data;
using TeamApp.Models;
using TeamApp.Services;
using TeamApp.ViewModels;

namespace TeamApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _dapper;
        private readonly Methods _methods;
        private readonly HttpClient _httpClient;  

        public LoginController(IConfiguration config, Methods methods, HttpClient httpClient)
        {
            _dapper = new ApplicationDbContext(config);
            _methods = methods;
            _httpClient = httpClient;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Index(LoginViewModel loginData)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string hashedPassword = _methods.HashPassword(loginData.Password);
        //        var user = _dapper.LoadSingleData<Users>("SELECT * FROM Users WHERE Email = @Email", new { Email = loginData.Email });


        //        if (user == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "This user does not exist!");
        //            return View(loginData);
        //        }


        //        var role = _dapper.LoadSingleData<Roles>("SELECT r.Name FROM Roles r JOIN UserRoles ur ON ur.RoleId = r.Id WHERE ur.UserId = @UserId", new { UserId = user.Id });

        //        if (user != null)
        //        {
        //            if (user.Email == loginData.Email && user.Password == hashedPassword)
        //            {

        //                HttpContext.Session.SetString("Id", user.Id);
        //                HttpContext.Session.SetString("Name", user.Name);
        //                if (role != null)
        //                {
        //                    HttpContext.Session.SetString("Role", role.Name);
        //                }
        //                HttpContext.Session.SetString("Surname", user.Surname);
        //                HttpContext.Session.SetString("Email", user.Email);
        //                HttpContext.Session.SetString("Team", user.Team);
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError(string.Empty, "The Email or Password are not correct.");
        //                return View(loginData);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Email", "User with this mail does not exist.");
        //            return View(loginData);
        //        }
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginData)
        {
            if (!ModelState.IsValid)
            {
                return View(loginData);
            }

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7267/api/Login/Login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                var message = result["message"];
                var userJson = result["user"]?.ToString(); 
                var role = result["role"]?.ToString();

                if (!string.IsNullOrEmpty(userJson))
                {

                    var user = JsonConvert.DeserializeObject<Users>(userJson);

                    HttpContext.Session.SetString("Id", user.Id.ToString());
                    HttpContext.Session.SetString("Name", user.Name);
                    HttpContext.Session.SetString("Surname", user.Surname);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("Team", user.Team);

                    if (!string.IsNullOrEmpty(role))
                    {
                        HttpContext.Session.SetString("Role", role);
                    }

                    return RedirectToAction("Index", "Home");

                }

                ModelState.AddModelError(string.Empty, "Login failed, please try again.");
                return View(loginData);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login failed. " + await response.Content.ReadAsStringAsync());
                return View(loginData);
            }
        }



        public IActionResult Logout() 
        {
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("Surname");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Team");

            return RedirectToAction("Index", "Home");
        }

    }
}
