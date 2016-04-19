DailyOps = window['DailyOps'] || {};

DailyOps.commands = (function () {
    var self = {

        createPlan: function (planId, name, type, ownerUserKey) {
            var model = {
                "id": planId,
                "sourceId": null,
                "name": name,
                "owner": ownerUserKey,
                "type": type,
                "tasks": [],
                "collaborators": [],
                "messages": [],
                "creationDate": new Date()
            };
            DailyOps.localStorage.appendPlan(model);
			
            var planInfo = {
                "id": planId,
                "name": name,
                "owner": ownerUserKey,
                "type": type,
                "creationDate": new Date()
            };
			
			return DailyOps.events.planCreated(planId, planInfo);
        },

        changePlanName: function (planId, newName) {
            var planModel = DailyOps.localStorage.planById(planId);
            var oldName = null;
            if (planModel) {
                oldName = planModel.name;
                planModel.name = newName;
                DailyOps.localStorage.updatePlan(planId, planModel);
            }
			
            var eventInfo = {
                "planId": planId,
                "name": newName,
                "oldName": oldName,
                "timestamp": new Date()
            };
			
			return DailyOps.events.planRenamed(planId, eventInfo);
        },


        collaboratorAdded: function (planId, username, role) {
            var collaborator = {
                "username": username,
                "role": role,
                "timestamp": new Date()
            };

            var planModel = DailyOps.localStorage.planById(planId);
            if (planModel) {
                planModel.collaborators[planModel.collaborators.length] = collaborator;
                DailyOps.localStorage.updatePlan(planId, planModel);
            }
            return collaborator;
        },

        addTask: function (planId, taskId, summary, repeatMode, creator) {
            var model = {
                "planId": planId,
                "taskId": taskId,
                "summary": summary,
                "repeatMode": repeatMode,
                "creator": creator,
                "completions": [],
                "creationDate": new Date()
            };

            DailyOps.localStorage.appendTask(planId, model);
			
			
            var eventInfo = {
                // "planId": planId,
                "taskId": taskId,
                "summary": summary,
                "repeatMode": repeatMode,
                "creator": creator,
                "creationDate": new Date()
            };
			
			return DailyOps.events.taskAdded(planId, eventInfo);
        },

        taskCompleted: function (planId, taskId, user) {
            var model = {
                "planId": planId,
                "taskId": taskId,
                "user": user,
                "timestamp": new Date()
            };

            var planModel = DailyOps.localStorage.planById(planId);
            if (planModel) {
                planModel.tasks.forEach(function (task, index, array) {
                    if (task.taskId === taskId) {
                        task.completions[task.completions.length] = model;
                    }
                });
                DailyOps.localStorage.updatePlan(planId, planModel);
            }
			
            var eventInfo = {
                "planId": planId,
                "taskId": taskId,
                "user": user,
                "timestamp": new Date()
            };
			
			return DailyOps.events.taskCompleted(planId, eventInfo);
        },


        taskCompletionRevoked: function (planId, taskId, user) {
            var model = {
                "planId": planId,
                "taskId": taskId,
                "user": user,
                "timestamp": new Date()
            };

            var planModel = DailyOps.localStorage.planById(planId);
            if (planModel) {
                planModel.tasks.forEach(function (task, index, array) {
                    if (task.taskId === taskId) {
                        task.completions.pop();
                    }
                });
                DailyOps.localStorage.updatePlan(planId, planModel);
            }
			
            var eventInfo = {
                "planId": planId,
                "taskId": taskId,
                "user": user,
                "timestamp": new Date()
            };
			
			return DailyOps.events.taskCompletionRevoked(planId, eventInfo);
        },

        removePlan: function (planId, reason) {
            var planModel = DailyOps.localStorage.planById(planId);
            if (planModel) {
                DailyOps.localStorage.removePlan(planId, reason);
            }
            // return planModel;
            var planInfo = {
                "planId": planId,
                "reason": reason,
                "timestamp": new Date()
            };
			return DailyOps.events.planRemoved(planId, planInfo);
        }

    };

    return self;

})();

