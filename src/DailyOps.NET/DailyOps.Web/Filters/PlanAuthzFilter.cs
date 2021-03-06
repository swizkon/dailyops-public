﻿using DailyOps.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DailyOps.Web.Filters
{
    public class PlanAuthorizeFilterAttribute : ActionFilterAttribute
    {
        public string ParamName { get; set; }
        public string[] AllowedRoles { get; set; }

        public PlanAuthorizeFilterAttribute()
        {
        }

        public PlanAuthorizeFilterAttribute(string paramName, string allowedRoles)
        {
            ParamName = paramName;
            AllowedRoles = allowedRoles.ToLower().Split(',');
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Try cast the value...
            var planIdValue = filterContext.ActionParameters[ParamName];

            // REVISIT Possible double convertion
            var planid = new PlanId(new Guid(planIdValue.ToString()));
            
            var allowedRoles = new List<string>(AllowedRoles)
                                    .ConvertAll(r => r.ToLower());

            // Get the roles for the current user...
            var collaboratorsInPlan = Wiring.Proxy.Collaborators.ByPlanId((Guid)planid);

            var currentUserRoles =
                collaboratorsInPlan
                    .Where(c => c.Username.StartsWith(filterContext.HttpContext.User.Identity.Name))
                    .ToList()
                    .ConvertAll(r => r.Role.ToLower());

            // Match against current user..start
            var userIsInRole = currentUserRoles.Intersect(allowedRoles).Any();

            if (!userIsInRole)
            {
                // Find the roles

                filterContext.Result = new HttpStatusCodeResult(
                    HttpStatusCode.BadRequest,
                    string.Format("The operation requires any of the following roles: {0}. The user has roles: {1}",
                        string.Join(",", allowedRoles),
                        string.Join(",", currentUserRoles)));
            }
        }



    }
}