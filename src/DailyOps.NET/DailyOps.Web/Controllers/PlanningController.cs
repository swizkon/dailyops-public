﻿using DailyOps.Commands;
using DailyOps.Domain;
using DailyOps.Web.Filters;
using DailyOps.Wiring.ReadModels;
using Nuclear.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;

namespace DailyOps.Web.Controllers
{
    [Authorize]
    public class PlanningController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet, ActionName("plans")]
        public ActionResult Plans()
        {
            var planIndex = Wiring.Proxy.Collaborators.PlansForUser(Thread.CurrentPrincipal.Identity);

            List<PlanDto> list = (List<PlanDto>)Wiring.Proxy.Plans.PlansWithId(planIndex);
            
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = list
            };
        }


        [HttpGet, ActionName("planDetails")]
        [PlanAuthorizeFilter("planId", "Owner,Admin,Collaborator,Auditor")]
        public ActionResult PlanDetails(string planId)
        {
            IEnumerable<PlanId> planIndex = Wiring.Proxy.Collaborators.PlansForUser(Thread.CurrentPrincipal.Identity);
            List<PlanDto> list = (List<PlanDto>)Wiring.Proxy.Plans.PlansWithId(planIndex);

            PlanDto pl = list.First(p => p.PlanId.ToString() == planId);

            var result = new
            {
                plan = pl,
                tasks = Wiring.Proxy.Tasks.TaskInPlan(pl.PlanId),
                collaborators = Wiring.Proxy.Collaborators.ByPlanId(pl.PlanId)
            };

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };
        }


        [HttpPost, ActionName("plans")]
        public ActionResult HandleCreatePlan(
            [System.Web.Http.FromBody] string planName,
            [System.Web.Http.FromBody] string planDescription = "",
            [System.Web.Http.FromBody] PlanType plantype = PlanType.Personal)
        {
            if (string.IsNullOrEmpty(planName))
                throw new ArgumentNullException(nameof(planName));

            var planId = PlanId.Create();
            var command = (plantype == PlanType.Collaborative)
                ? new CreateCollaborativePlan(planId, planName, planDescription, Thread.CurrentPrincipal.Identity.Name)
                : (plantype == PlanType.Personal)
                    ? new CreatePersonalPlan(planId, planName, planDescription, Thread.CurrentPrincipal.Identity.Name)
                    : new CreateDistributablePlan(planId, planName, planDescription, Thread.CurrentPrincipal.Identity.Name) as Command;
            
            Wiring.Proxy.SendCommand(command);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };
        }



        [HttpPost, ActionName("tasks")]
        [PlanAuthorizeFilter("plan", "Owner,Admin,Collaborator")]
        public ActionResult HandleCreateTask(
            [System.Web.Http.FromBody] Guid plan, 
            [System.Web.Http.FromBody] string taskTitle)
        {
            if (string.IsNullOrEmpty(taskTitle))
                throw new ArgumentNullException(nameof(taskTitle));

            var planId = new PlanId(plan);
            var taskId = (TaskId)Guid.NewGuid();

            var command = new CreateTask(planId, taskId, taskTitle, Reccurence.Daily);

            Wiring.Proxy.SendCommand(command);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };
        }


        [HttpPost, ActionName("collaborators")]
        [PlanAuthorizeFilter("plan", "Admin")]
        public ActionResult HandleAddCollaborator(
            [System.Web.Http.FromBody] Guid plan,
            [System.Web.Http.FromBody] string collaboratorUsername,
            [System.Web.Http.FromBody] string collaboratorRole)
        {
            if (string.IsNullOrEmpty(collaboratorUsername))
                throw new ArgumentNullException(nameof(collaboratorUsername));

            if (string.IsNullOrEmpty(collaboratorRole))
                throw new ArgumentNullException(nameof(collaboratorRole));

            
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ModelState.Values.ToString());
            }


            var planId = new PlanId(plan);

            var command = new AddCollaborator(planId, collaboratorUsername, collaboratorRole);

            Wiring.Proxy.SendCommand(command);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };
        }

    }
}
