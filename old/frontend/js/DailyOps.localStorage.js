DailyOps = window['DailyOps'] || {};

DailyOps.localStorage = (function () {

    if (!window.localStorage) {
        alert("Local storage not available");
        return null;
    }

    // Try to read the cache object from localStorage...

    return {
        allPlans: function (then, error) {

            var planRepository = window["planRepository"] || JSON.parse(window.localStorage.getItem("planRepository") || "{}");

            planRepository["plans"] = planRepository["plans"] || [];
            var plans = planRepository["plans"];

            then(200, "OK", plans);
        },
        plans: function (userKey, then, error) {

            var planRepository = window["planRepository"] || JSON.parse(window.localStorage.getItem("planRepository") || "{}");

            planRepository["plans"] = planRepository["plans"] || [];
            var plans = $.grep(planRepository["plans"], function (a, b) {
                // TODO Also check for a collaborator or something...
                return a.owner == userKey;
            });
            var model = {
                "userKey": userKey,
                "plans": plans
            };
            then(200, "OK", model);
            // Do a then if found, else "error"
            // return true;
        },

        planById: function (planId) {
            var planRes = null;
            DailyOps.localStorage.allPlans(function (status, statusText, plans) {

                planRes = $.grep(plans, function (a, b) {
                    return a.id == planId;
                });
            });
            // TODO Check for zero result exception...
            return planRes[0];
        },

        tasks: function (planId, then) {

            var plan = DailyOps.localStorage.planById(planId);
            if (plan) {

                var tasksModel = {
                    "planId": planId,
                    "tasks": plan.tasks
                };

                console.log(tasksModel);
                then(200, "OK", tasksModel);
                return;
            }
        },

        userEnabledTasks: function (userKey, then) {


            DailyOps.localStorage.plans(app.currentUser.userKey,
                function (status, statusText, plans) {
                    var tasks = [];
                    $.each(plans["plans"], function (a, plan) {
                        $.each(plan["tasks"], function (a, task) {
                            tasks[tasks.length] = {
                                "task": task.summary,
                                "repeatMode": task.repeatMode,
                                "taskId": task.taskId,
                                "plan": plan.name,
                                "planId": task.planId,
                                "completions": task.completions
                            };
                        });
                    });

                    if (then) {
                        then({ "body": tasks });
                    }
                });
        },

        appendPlan: function (planModel) {

            var cacheObj = JSON.parse(window.localStorage.getItem("planRepository") || "{}");
            cacheObj["plans"] = cacheObj["plans"] || [];

            cacheObj["plans"][cacheObj["plans"].length] = planModel;
            window["planRepository"] = cacheObj;
            window.localStorage.setItem("planRepository", JSON.stringify(cacheObj));
        },

        updatePlan: function (planId, planModel) {

            var planRepository = window["planRepository"] || JSON.parse(window.localStorage.getItem("planRepository") || "{}");
            var plans = planRepository["plans"] || [];

            var planIndex = planRepository["plans"].length;
            $.each(planRepository["plans"], function (pos, obj) {
                if (obj.id === planId) {
                    planIndex = pos;
                }
            });
            plans[planIndex] = planModel;
            planRepository["plans"] = plans;

            window["planRepository"] = planRepository;
            window.localStorage.setItem("planRepository", JSON.stringify(planRepository));
        },

        appendTask: function (planId, taskModel) {
            var plan = DailyOps.localStorage.planById(planId);
            if (plan && plan.tasks) {
                plan.tasks[plan.tasks.length] = taskModel;
                DailyOps.localStorage.updatePlan(planId, plan);
                return;
            }
        },


        removePlan: function (planId, reason) {
            var planRepository = window["planRepository"] || JSON.parse(window.localStorage.getItem("planRepository") || "{}");
            var currentPlans = planRepository["plans"] || [];
            var i;
            var newPlans = [];
            for (i = 0; i < currentPlans.length; i++) {
                if (currentPlans[i].id !== planId) {
                    newPlans[newPlans.length] = currentPlans[i];
                }
            }
            planRepository["plans"] = newPlans;
            window["planRepository"] = planRepository;
            window.localStorage.setItem("planRepository", JSON.stringify(planRepository));
        }
    };
})();
