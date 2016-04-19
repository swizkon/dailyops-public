using DailyOps.Wiring.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DailyOps.Web.Controllers
{
    public class ExecutionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet, ActionName("completedTasks")]
        public ActionResult CompletedTasks()
        {
            // ICollection<CompletedTaskDto> 
            var list = new List<CompletedTaskDto>(10);
            for (int i = 0; i < 13; i++)
            {
                list.Add(
                    new CompletedTaskDto {
                         CompletedBy = "jonas", 
                         LastCompleted = DateTime.Now.ToString(), PlanId = Guid.NewGuid(), TaskId = Guid.NewGuid(), Title = "COmpleted task #" + i
                    });
            }

            var result = new JsonResult()
            {
                 JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                 Data = list
            };
            return result; // new List<CompletedTaskDto>();
        }


        [HttpGet, ActionName("remainingTasks")]
        public ActionResult RemainingTasks()
        {
            var list = new List<RemainingTaskDto>();
            for (int i = 0; i < 13; i++)
            {
                list.Add(
                    new RemainingTaskDto
                    {
                        PlanName = "Plan name", 
                        TaskType = "Daily",
                        TaskId = Guid.NewGuid(),
                        Title = "Remaining task #" + i
                    });
            }

            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = list
            };
        }
    }
}
