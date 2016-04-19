using DailyOps.Commands;
using DailyOps.Domain;
using DailyOps.Wiring.ReadModels;
using Nuclear.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
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
            List<PlanDto> list = (List<PlanDto>)Wiring.Proxy.Plans.ForCurrentUser(Thread.CurrentPrincipal.Identity);
            
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = list
            };
        }


        [HttpGet, ActionName("planDetails")]
        public ActionResult PlanDetails(string planId)
        {
            List<PlanDto> list = (List<PlanDto>)Wiring.Proxy.Plans.ForCurrentUser(Thread.CurrentPrincipal.Identity);

            PlanDto pl = list.Where(p => p.PlanId.ToString() == planId).First();

            var result = new {
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
            if (String.IsNullOrEmpty(planName))
                throw new ArgumentNullException("planName");

            var planId = new PlanId();

            

            var command = (plantype == PlanType.Collaborative)
                ? (Command)new CreateCollaborativePlan(planId, planName, planDescription, Thread.CurrentPrincipal.Identity.Name)
                : (plantype == PlanType.Personal)
                    ? (Command)new CreatePersonalPlan(planId, planName, planDescription, Thread.CurrentPrincipal.Identity.Name)
                    : (Command)new CreateDistributablePlan(planId, planName, planDescription, Thread.CurrentPrincipal.Identity.Name);

            // = new CreateSharedPlan(planId, planName, Thread.CurrentPrincipal.Identity.Name);

            Wiring.Proxy.SendCommand(command);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };
        }



        [HttpPost, ActionName("tasks")]
        public ActionResult HandleCreateTask([System.Web.Http.FromBody] Guid plan, [System.Web.Http.FromBody] string taskTitle)
        {
            if (String.IsNullOrEmpty(taskTitle))
                throw new ArgumentNullException("taskTitle");

            var planId = new PlanId(plan);
            var taskId = new TaskId();

            var command = new CreateTask(planId, taskId, taskTitle, TaskType.Daily);

            Wiring.Proxy.SendCommand(command);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };
        }


        [HttpPost, ActionName("collaborators")]
        public ActionResult HandleAddCollaborator([System.Web.Http.FromBody] Guid plan, [System.Web.Http.FromBody] string collaboratorEmail)
        {
            if (String.IsNullOrEmpty(collaboratorEmail))
                throw new ArgumentNullException("collaboratorEmail");

            var planId = new PlanId(plan);

            var command = new AddCollaborator(planId, collaboratorEmail, "Admin");

            Wiring.Proxy.SendCommand(command);

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = command
            };
        }

    }
}
