using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using System.Text;
using TeamApp.Data;
using TeamApp.Models;
using TeamApp.Services;

namespace TeamApp.Controllers
{
    public class EquipmentController : Controller
    {

        private readonly ApplicationDbContext _dapper;
        private readonly Methods _methods;
        private readonly HttpClient _httpClient;

        public EquipmentController(IConfiguration config, Methods methods, HttpClient httpClient)
        {
            _dapper = new ApplicationDbContext(config);
            _methods = methods;
            _httpClient = httpClient;
        }





        //public IActionResult Index()
        //{

        //    var data = _dapper.LoadData<Equipment>("SELECT * FROM Equipment WHERE UserId = @UserId", new { UserId = HttpContext.Session.GetString("Id") });


        //    return View(data);
        //}

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("Id");

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not logged in." });
            }

            var response = await _httpClient.GetAsync($"https://localhost:7267/api/Equipment?userId={userId}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<Equipment>>();
                return View(data);  
            }
            else
            {
                return View(new List<Equipment>());
            }

        }








        //[HttpGet]
        //[Route("/Equipment/Edit/{id}")]
        //public IActionResult Edit(int id)
        //{
        //    var data = _dapper.LoadSingleData<Equipment>("SELECT * FROM Equipment WHERE Id = @Id", new { Id = id });

        //    return View(data);
        //}


        [HttpGet]
        [Route("/Equipment/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            if(id == null)
            {
                return NotFound(new { message = "Item not found!" });
            }

            var response = await _httpClient.GetAsync($"https://localhost:7267/api/Equipment/Edit/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Equipment>();
                return View(data);
            }
            else
            {
                return View(new Equipment());
            }
        }








        //[HttpPost]
        //public IActionResult Edit(Equipment eqp)
        //{

        //    _dapper.ExecuteQuery("UPDATE Equipment SET TypeOfEqp = @TypeOfEqp, Model = @Model, SerialNumber = @SerialNumber, InUse = @InUse, UserId = @UserId WHERE Id = @Id", new { TypeOfEqp = eqp.TypeOfEqp, Model = eqp.Model, SerialNumber = eqp.SerialNumber, InUse = eqp.InUse, UserId = eqp.UserId, Id = eqp.Id });


        //    return RedirectToAction("Index");
        //}


        [HttpPost]
        public async Task<IActionResult> Edit(Equipment eqp)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7267/api/Equipment/Edit", eqp);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to update equipment.");
                return View(eqp);
            }


        }







        //[HttpGet]
        //[Route("/Equipment/Delete/{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var data = _dapper.LoadSingleData<Equipment>("SELECT * FROM Equipment WHERE Id = @Id" , new { Id = id });


        //    return View(data);
        //}

        [HttpGet]
        [Route("/Equipment/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == null)
            {
                return NotFound(new {message = "Item not found!"});
            }

            var response = await _httpClient.GetAsync($"https://localhost:7267/api/Equipment/Delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Equipment>();
                return View(data);
            }
            else
            {
                return View(new Equipment());
            }
        }






        //[HttpPost]
        //public IActionResult Delete(Equipment eqp)
        //{
        //    var data = _dapper.ExecuteQuery("DELETE FROM Equipment WHERE Id = @Id", new { Id = eqp.Id });


        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        [Route("/Equipment/DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7267/api/Equipment/DeleteItem/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }





        //[HttpGet]
        //public IActionResult Add()
        //{
        //    var data = _dapper.SelectAll<Users>("SELECT * FROM Users");

        //    return View(data);
        //}

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var response = await _httpClient.GetAsync("https://localhost:7267/api/Equipment/Add");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<Users>>();
                return View(data);
            }
            else
            {
                return View(new Users());
            }
        }


        //[HttpPost]
        //public IActionResult Add(Equipment eqp)
        //{

        //    _dapper.ExecuteQuery("INSERT INTO Equipment(TypeOfEqp, Model, SerialNumber, InUse, UserId) VALUES(@TypeOfEqp, @Model, @SerialNumber, @InUse, @UserId)", new { TypeOfEqp = eqp.TypeOfEqp, @Model = eqp.Model, @SerialNumber = eqp.SerialNumber, InUse = eqp.InUse, UserId = eqp.UserId });


        //    return RedirectToAction("Index");
        //}


        [HttpPost]
        public async Task<IActionResult> Add (Equipment eqp)
        {
            if(eqp == null)
            {
                return NotFound(new {message = "COuld not be added!"});
            }

            var json = JsonConvert.SerializeObject(eqp); 
            var content = new StringContent(json, Encoding.UTF8, "application/json");




            var response = await _httpClient.PostAsync("https://localhost:7267/api/Equipment/AddItem", content);

            return RedirectToAction("Index");
        }

    }
}
