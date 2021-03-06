﻿using DailyOps.Commands;
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

        public ActionResult React()
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
            TaskId taskId = (TaskId) Guid.NewGuid();
            var command = new MarkTaskCompleted(taskId, User.Identity.Name, timestamp);

            Wiring.Proxy.SendCommand(command);

            return CommandWrapped(command);
        }


        [HttpPost, ActionName("revokedTasks")]
        [TaskAuthorizationFilter("task", "Admin,Collaborator")]
        public ActionResult HandleRevokeTask(
            [System.Web.Http.FromBody] Guid task,
            [System.Web.Http.FromBody] string revocationTimestamp = "")
        {
            TaskId taskId = (TaskId)Guid.NewGuid();
            var command = new RevokeTaskCompletion(taskId, User.Identity.Name, revocationTimestamp);

            Wiring.Proxy.SendCommand(command);
            return CommandWrapped(command);
        }

        private JsonResult CommandWrapped(Nuclear.Messaging.Command command)
        {
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };
        }

    }
}
