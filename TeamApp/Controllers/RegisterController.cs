using Microsoft.AspNetCore.Mvc;
using TeamApp.Data;
using TeamApp.Models;
using TeamApp.Services;
using TeamApp.ViewModels;

namespace TeamApp.Controllers
{
    public class RegisterController : Controller
    {

        private readonly ApplicationDbContext _dapper;
        private readonly Methods _methods;

        public RegisterController(IConfiguration config, Methods methods) 
        {
            _dapper = new ApplicationDbContext(config);
            _methods = methods;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registration(RegistrationViewModel data)
        {
            if (ModelState.IsValid)
            {

                var existingUser = _dapper.LoadSingleData<Users>(@"SELECT * FROM Users WHERE Email = @Email", new { Email = data.Email });

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This Email already exists.");
                    return View("Index", data);
                }

                string userId = Guid.NewGuid().ToString();

                string hashedId = _methods.HashPassword(userId);

                var user = new Users
                {
                    Id = hashedId,
                    Name = data.Name,
                    Surname = data.Surname,
                    BirthDay = data.BirthDay,
                    Email = data.Email,
                    Password = _methods.HashPassword(data.Password),
                    Position = data.Position,
                    Team = data.Team,
                    JoinedAt = DateTime.Now
                };




                string sql = @"INSERT INTO Users (Id, Name, Surname, BirthDay, Email, Password, Position, Team, JoinedAt) 
            VALUES (@Id, @Name, @Surname, @BirthDay, @Email, @Password, @Position, @Team, @JoinedAt)";

                var parameters = new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    BirthDay = user.BirthDay,
                    Email = user.Email,
                    Password = user.Password,
                    Position = user.Position,
                    Team = user.Team,
                    JoinedAt = user.JoinedAt
                };
                _dapper.ExecuteQuery(sql, parameters);

                return View("~/Views/Partials/RegistrationSuccess.cshtml");
               
            }
            else
            {
                return View("Index", data);
            }
        }
    }
}
