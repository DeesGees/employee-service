using Microsoft.AspNetCore.Mvc;
using TeamApp.Data;
using TeamApp.Models;
using TeamApp.Services;

namespace TeamApp.Controllers
{
    public class EquipmentController : Controller
    {

        private readonly ApplicationDbContext _dapper;
        private readonly Methods _methods;

        public EquipmentController(IConfiguration config, Methods methods)
        {
            _dapper = new ApplicationDbContext(config);
            _methods = methods;
        }

        public IActionResult Index()
        {

            var data = _dapper.LoadData<Equipment>("SELECT * FROM Equipment WHERE UserId = @UserId", new { UserId = HttpContext.Session.GetString("Id") });


            return View(data);
        }


        [HttpGet]
        [Route("/Equipment/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var data = _dapper.LoadSingleData<Equipment>("SELECT * FROM Equipment WHERE Id = @Id", new { Id = id });

            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(Equipment eqp)
        {

            _dapper.ExecuteQuery("UPDATE Equipment SET TypeOfEqp = @TypeOfEqp, Model = @Model, SerialNumber = @SerialNumber, InUse = @InUse, UserId = @UserId WHERE Id = @Id", new { TypeOfEqp = eqp.TypeOfEqp, Model = eqp.Model, SerialNumber = eqp.SerialNumber, InUse = eqp.InUse, UserId = eqp.UserId, Id = eqp.Id });


            return RedirectToAction("Index");
        }



        [HttpGet]
        [Route("/Equipment/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var data = _dapper.LoadSingleData<Equipment>("SELECT * FROM Equipment WHERE Id = @Id" , new { Id = id });


            return View(data);
        }


        [HttpPost]
        public IActionResult Delete(Equipment eqp)
        {
            var data = _dapper.ExecuteQuery("DELETE FROM Equipment WHERE Id = @Id", new { Id = eqp.Id });


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {
            var data = _dapper.SelectAll<Users>("SELECT * FROM Users");

            return View(data);
        }


        [HttpPost]
        public IActionResult Add(Equipment eqp)
        {

            _dapper.ExecuteQuery("INSERT INTO Equipment(TypeOfEqp, Model, SerialNumber, InUse, UserId) VALUES(@TypeOfEqp, @Model, @SerialNumber, @InUse, @UserId)", new { TypeOfEqp = eqp.TypeOfEqp, @Model = eqp.Model, @SerialNumber = eqp.SerialNumber, InUse = eqp.InUse, UserId = eqp.UserId });


            return RedirectToAction("Index");
        }

    }
}
