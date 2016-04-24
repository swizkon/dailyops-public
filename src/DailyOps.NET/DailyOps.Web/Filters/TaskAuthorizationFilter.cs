using DailyOps.Domain;
using DailyOps.Wiring.ReadModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DailyOps.Web.Filters
{
    public class TaskAuthorizationFilterAttribute : ActionFilterAttribute
    {
        public string ParamName { get; set; }
        public string[] AllowedRoles { get; set; }

        public TaskAuthorizationFilterAttribute(string paramName, string allowedRoles)
        {
            this.ParamName = paramName;
            this.AllowedRoles = allowedRoles.Split(',');
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Try cast the value...
            var taskIdValue = filterContext.ActionParameters[this.ParamName];

            // REVISIT Possible double convertion
            TaskId taskId = new TaskId(new Guid(taskIdValue.ToString()));

            TaskDto taskModel = Wiring.Proxy.Tasks.TaskWithId(taskId);

            Converter<string, string> conv = (a) => { return a.ToLower(); };
            List<string> allowedRoles = new List<string>( Array.ConvertAll<string, string>(this.AllowedRoles, conv));

            // Get the roles for the current user...
            var collaboratorsInPlan = Wiring.Proxy.Collaborators.ByPlanId(taskModel.PlanId);

            // Match against current user.
            bool userIsInRole = collaboratorsInPlan.Count(c =>
                {
                    return (c.Username == filterContext.HttpContext.User.Identity.Name
                        || c.DisplayName == filterContext.HttpContext.User.Identity.Name.Replace("(guest)",""))
                        && allowedRoles.Contains(c.Role.ToLower());
                }) > 0;

            if (!userIsInRole)
            {
                filterContext.Result = new HttpStatusCodeResult(
                                            HttpStatusCode.BadRequest,
                                            "The operation requires any of the following roles: " + String.Join(",", this.AllowedRoles));
            }
        }



    }
}