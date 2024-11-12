using Microsoft.AspNetCore.Mvc;
using TeamApp.Data;
using TeamApp.Models;

namespace TeamApp.Controllers
{
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _dapper;

        public TeamController(IConfiguration config)
        {
            _dapper = new ApplicationDbContext(config);
        }




        public IActionResult Index()
        {
            var user = _dapper.LoadSingleData<Users>("SELECT * FROM Users WHERE Name = @Name", new { Name = HttpContext.Session.GetString("Name") });
            List<Users> myTeam = _dapper.LoadData<Users>("Select * FROM Users WHERE Team = @Team", new { Team = user.Team}).ToList();

            return View(myTeam);
        }

        [HttpGet]
        public JsonResult GetUserDetails(string id)
        {
            var user = _dapper.LoadSingleData<Users>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
            var tasks = _dapper.LoadData<Tasks>("SELECT * FROM Tasks WHERE UserId = @UserId", new { UserId = id });

            if (user == null)
            {
                return Json(new { error = "User not found" });
            }

            var taskDetails = tasks.Select(task => new
            {
                taskHeader = task.TaskHeader,
                taskDescription = task.TaskDescription,
                taskAddedDate = task.TaskAddedDate,
                taskStatus = task.TaskStatus
            }).ToList();


            var result = new
            {
                name = user.Name,
                surname = user.Surname,
                position = user.Position,
                team = user.Team,
                email = user.Email,
                tasks = taskDetails
            };

            return Json(result);
        }


        [HttpGet]
        [Route("/Team/Equipment/{id}")]
        public IActionResult Equipment(string id)
        {
            var data = _dapper.LoadData<Equipment>("SELECT * FROM Equipment Where UserId = @UserId", new { UserId = id });


            return View(data);
        }
    }
}
