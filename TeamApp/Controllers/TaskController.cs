using Microsoft.AspNetCore.Mvc;
using TeamApp.Data;
using TeamApp.Models;
using TeamApp.ViewModels;

namespace TeamApp.Controllers
{
    public class TaskController : Controller
    {

        private readonly ApplicationDbContext _dapper;

        public TaskController(IConfiguration config)
        {
            _dapper = new ApplicationDbContext(config);
        }


        public IActionResult Index()
        {

            if(HttpContext.Session.GetString("Name") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _dapper.LoadData<Users>("SELECT * FROM Users WHERE Name = @Name", new { Name = HttpContext.Session.GetString("Name")}).ToList();


            var data = new UserTasksViewModel
            {
                Users = user,
                Task = new Tasks()
            };


            return View(data);
        }

        public IActionResult Tasks(string id, int? status)
        {

            if (HttpContext.Session.GetString("Name") == null && (HttpContext.Session.GetString("Role") == "Team Leader" || HttpContext.Session.GetString("Role") == "Administrator"))
            {
                return RedirectToAction("Index", "Login");
            }

            string sql = "SELECT * FROM Tasks WHERE UserId = @UserId";

            if (status.HasValue)
            {
                sql = "SELECT * FROM Tasks WHERE UserId = @UserId AND TaskStatus = @Status";
            }

            ViewBag.UserId = id;

            var parameters = new { UserId = id, Status = status };
            List<Tasks> tasks = _dapper.LoadData<Tasks>(sql, parameters).ToList();

            return View(tasks);
        }


        public IActionResult AddTask(string id)
        {
            Users user = _dapper.LoadSingleData<Users>("SELECT * FROM Users Where Id = @Id", new { Id = id });


            return View(user);
        }

        [HttpPost]
        public IActionResult SaveTask(Tasks task)
        {
            if (ModelState.IsValid)
            {
                int saveNewTask = _dapper.ExecuteQuery("INSERT INTO Tasks(UserName, UserSurname, UserId, TaskHeader, TaskDescription, TaskAddedDate, TaskStatus) VALUES(@UserName, @UserSurname, @UserId, @TaskHeader, @TaskDescription, @TaskAddedDate, @TaskStatus)", new { UserName = task.UserName, UserSurname = task.UserSurname, UserId = task.UserId, TaskHeader = task.TaskHeader, TaskDescription = task.TaskDescription, TaskAddedDate = task.TaskAddedDate, TaskStatus = 2 });
                return RedirectToAction("Index");
            }
            


            return View(task);
        }


        [HttpPost]
        public IActionResult NewTask(Tasks task)
        {
            if(ModelState.IsValid)
            {
                _dapper.ExecuteQuery("INSERT INTO Tasks(UserName, UserSurname, UserId, TaskHeader, TaskDescription, TaskAddedDate, TaskStatus) VALUES(@UserName, @UserSurname, @UserId, @TaskHeader, @TaskDescription, @TaskAddedDate, @TaskStatus)", new { UserName = task.UserName, UserSurname = task.UserSurname, UserId = task.UserId, TaskHeader = task.TaskHeader, TaskDescription = task.TaskDescription, TaskAddedDate = task.TaskAddedDate, TaskStatus = 0 });
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult MyTasks()
        {
            List<Tasks> data = _dapper.LoadData<Tasks>("SELECT * FROM Tasks WHERE UserId = @UserId", new { UserId = HttpContext.Session.GetString("Id") }).ToList();



            return View(data);
        }
    }
}
