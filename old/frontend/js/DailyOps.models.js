DailyOps = window['DailyOps'] || {};

DailyOps.models = (function () {

    var self = {
        planModel: function () {
            return {
                "id": "",
                "sourceId": "", // 
                "name": "The name of the plan",
                "owner": "userKey",
                "type": "personal",
                "collaborators": [],
                "tasks": [],
                "events": [],
                "messages": []
            }
        },
        taskModel: function () {
            return {
                "id": "",
                "summary": "A short desc",
                "repeatMode": "A short desc",
                "extensions": null
            }
        },
        planEventModel: function () {
            return {
                "planId": "",
                "taskId": "",
                "eventName": "",
                "timestamp": new Date()
            }
        },
        messageModel: function () {
            return {
                "message": "",
                "taskId": "",
                "eventName": "",
                "timestamp": new Date()
            }
        }
    };

    return self;

})();

