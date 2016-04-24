using DailyOps.Commands;
using DailyOps.Domain;
using DailyOps.Web.Filters;
using DailyOps.Wiring.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyOps.Web.Controllers
{
    [Authorize]
    public class ExecutionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, ActionName("allTasks")]
        public ActionResult AllTasks()
        {
            var allPlansForCurrentUser = Wiring.Proxy.Collaborators.PlansForUser(HttpContext.User.Identity);

            List<TaskDto> list = Wiring.Proxy.Tasks.TasksInPlans(allPlansForCurrentUser);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = list
            };
        }


        [HttpGet, ActionName("completedTasks")]
        public ActionResult CompletedTasks()
        {
            var allPlansForCurrentUser = Wiring.Proxy.Collaborators.PlansForUser(HttpContext.User.Identity);

            List<CompletedTaskDto> mostRecentCompletions
                = Wiring.Proxy.Tasks.CompletedTasksInPlans(allPlansForCurrentUser);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = mostRecentCompletions
            };
            /*
            // ICollection<CompletedTaskDto> 
            var list = new List<CompletedTaskDto>(10);
            for (int i = 0; i < 13; i++)
            {
                list.Add(
                    new CompletedTaskDto {
                         CompletedBy = "jonas", 
                         LastCompleted = DateTime.Now.ToString(), 
                         PlanId = Guid.NewGuid(),
                         TaskId = Guid.NewGuid(), 
                         Title = "COmpleted task #" + i
                    });
            }

            var result = new JsonResult()
            {
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                 Data = list
            };
            return result;
            */
        }


        [HttpGet, ActionName("remainingTasks")]
        public ActionResult RemainingTasks()
        {
            var allPlansForCurrentUser = Wiring.Proxy.Collaborators.PlansForUser(HttpContext.User.Identity);

            var list = new List<RemainingTaskDto>();
            for (int i = 0; i < 13; i++)
            {
                list.Add(
                    new RemainingTaskDto
                    {
                        PlanName = "Plan name", 
                        TaskType = "Daily",
                        TaskId = Guid.NewGuid(),
                        TaskTitle = "Remaining task #" + i
                    });
            }

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = list
            };
        }


        [HttpPost, ActionName("completedTasks")]
        [TaskAuthorizationFilter("task", "Admin,Collaborator")]
        public ActionResult HandleCompleteTask(
            [System.Web.Http.FromBody] Guid task,
            [System.Web.Http.FromBody] string localTimestamp)
        {
            DateTimeOffset timestamp = DateTimeOffset.Now;
            // DateTimeOffset.Parse(localTimestamp);
            TaskId taskId = new TaskId(task);
            var command = new MarkTaskCompleted(taskId, User.Identity.Name, timestamp);

            Wiring.Proxy.SendCommand(command);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };

        }

    }
}
