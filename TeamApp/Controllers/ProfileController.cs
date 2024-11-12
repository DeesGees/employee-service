using Microsoft.AspNetCore.Mvc;
using System.Data;
using TeamApp.Data;
using TeamApp.Models;
using TeamApp.Services;
using TeamApp.ViewModels;

namespace TeamApp.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _dapper;
        private readonly Methods _methods;

        public ProfileController(IConfiguration config, Methods mthods) 
        {
            _dapper = new ApplicationDbContext(config);
            _methods = mthods;
        }
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("Name") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _dapper.LoadSingleData<Users>(@"SELECT * FROM Users where Name = @Name", new {Name = HttpContext.Session.GetString("Name") });
            return View(user);
        }

        [HttpGet]
        public IActionResult Roles()
        {
            if (HttpContext.Session.GetString("Role") != "Administrator")
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Name") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View("~/Views/Roles/Index.cshtml");
        }

        [HttpPost]
        public IActionResult Roles(RoleViewModel roles)
        {
            if (HttpContext.Session.GetString("Role") != "Administrator")
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingRole = _dapper.LoadSingleData<Roles>(@"SELECT * FROM Roles WHERE Name = @Name", new {Name = roles.Name});
                if(existingRole != null)
                {
                    ModelState.AddModelError("Name", "This role already exists!");
                    return View("~/Views/Roles/Index.cshtml", roles);
                }

                string roleId = Guid.NewGuid().ToString();
                string hashedRoleId = _methods.HashPassword(roleId);

                var newRole = new Roles
                {
                    Id = hashedRoleId,
                    Name = roles.Name,
                };

                _dapper.ExecuteQuery("INSERT INTO Roles(Id, Name) VALUES (@Id, @Name)", newRole);

                return RedirectToAction("Index", "Profile");
            }

            return View("~/Views/Roles/Index.cshtml");
        }


        [HttpGet]
        public IActionResult GetUsers()
        {

            if (HttpContext.Session.GetString("Role") != "Administrator")
            {
                return NotFound();
            }

            List<Users> users = _dapper.SelectAll<Users>("SELECT * FROM Users").ToList();
            List<Roles> roles = _dapper.SelectAll<Roles>("SELECT * FROM Roles").ToList();


            var data = new UserAndRolesViewModel
            {
                Role = roles,
                User = users 
            };

            return View("Users", data);

        }


        [HttpGet]
        [Route("/Profile/Edit/{id}")]
        public IActionResult Edit(string id)
        {

            if (HttpContext.Session.GetString("Role") != "Administrator")
            {
                return NotFound();
            }


            var selectUser = _dapper.LoadSingleData<Users>(@"SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            var selectRoles = _dapper.SelectAll<Roles>("SELECT * FROM Roles");


            var data = new UserAndRolesViewModel
            {
                SingleUser = selectUser,
                Role = selectRoles.ToList()
            };
            return View("Edit", data);
        }


        [HttpPost]
        public IActionResult Edit(UserAndRolesViewModel data)
        {

            if (HttpContext.Session.GetString("Role") != "Administrator")
            {
                return NotFound();
            }


            ModelState.Remove("Role");
            ModelState.Remove("User");
            ModelState.Remove("SingleUser");
            ModelState.Remove("SingleRoles");

            var existingUser = _dapper.LoadSingleData<UserRoles>(@"SELECT * FROM UserRoles WHERE UserId = @UserId", new { UserId = data.SelectedUserId });

            if (ModelState.IsValid)
            {
                if(existingUser == null) { 
                    _dapper.ExecuteQuery("INSERT INTO UserRoles(UserId, RoleId) VALUES(@UserId, @RoleId)", new { UserId = data.SelectedUserId, RoleId = data.SelectedRole });
                }
            }
            return RedirectToAction("GetUsers");
        }

        [HttpPost]
        public IActionResult ChangeRole(UserAndRolesViewModel data)
        {

            if (HttpContext.Session.GetString("Role") != "Administrator")
            {
                return NotFound();
            }


            ModelState.Remove("Role");
            ModelState.Remove("User");
            ModelState.Remove("SingleUser");
            ModelState.Remove("SingleRoles");

            var existingUser = _dapper.LoadSingleData<UserRoles>(@"SELECT * FROM UserRoles WHERE UserId = @UserId", new { UserId = data.SelectedUserId });


            if (ModelState.IsValid)
            {
                if(existingUser != null)
                {
                    _dapper.ExecuteQuery(@"UPDATE UserRoles SET RoleId = @RoleId WHERE UserId = @UserId", new { RoleId = data.SelectedRole, UserId = data.SelectedUserId });
                }
                return RedirectToAction("GetUsers");
            }
            return RedirectToAction("Edit");
        }


        [HttpGet]
        public IActionResult ChangePassword(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            string hashedPassword = _methods.HashPassword(oldPassword);
            string hashedNewPassword = _methods.HashPassword(newPassword);
            string hashedConfirmPassword = _methods.HashPassword(confirmPassword);


            var user = _dapper.LoadSingleData<Users>("SELECT * FROM Users WHERE Password = @Password AND Id = @Id", new { Password = hashedPassword, Id = HttpContext.Session.GetString("Id") });

            if (user != null && hashedNewPassword == hashedConfirmPassword) 
            {
                _dapper.ExecuteQuery("UPDATE Users SET Password = @NewPassword WHERE Id = @Id", new { NewPassword = hashedNewPassword, Id = user.Id });
                return View("Succes");
            }


            return RedirectToAction("Index");
        }
    }
}
