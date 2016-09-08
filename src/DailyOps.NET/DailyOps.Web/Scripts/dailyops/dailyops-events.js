
DailyOps = window['DailyOps'] || {};

DailyOps.eventstore = {

    host: "http://localhost",
    port: "2113",

    generateUUID: function () {

        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    },

    appendToStream: function (streamName, eventType, eventBody) { // , callback
        var eventData = [
        {
            "eventId": DailyOps.eventstore.generateUUID(),
            "eventType": eventType,
            "data": eventBody
        }
        ];

        // console.info("Send " + eventType + " to: " + streamName);
        return $.when($.ajax({
            url: DailyOps.eventstore.host + ":" + DailyOps.eventstore.port + "/streams/" + streamName,
            type: "POST",
            data: JSON.stringify(eventData),
            contentType: "application/vnd.eventstore.event+json"
        }));
    }
};



DailyOps.events = (function () {

    return {

        userCreated: function (username, displayName) {

            // Return a promise
            var formProps = {
                "username": username || "",
                "displayName": displayName || ""
            };
            return DailyOps.eventstore.appendToStream("users", "userCreated", formProps);
        },
        userDeactivated: function (username) {
            // Return a promise
            var formProps = {
                "username": username || ""
            };
            return DailyOps.eventstore.appendToStream("users", "userDeactivated", formProps);
        },
        userReactivated: function (username) {
            // Return a promise
            var formProps = {
                "username": username || ""
            };
            return DailyOps.eventstore.appendToStream("users", "userReactivated", formProps);
        },

        userDeleted: function (username) {
            // Return a promise
            var formProps = {
                "username": username || ""
            };
            return DailyOps.eventstore.appendToStream("users", "userDeleted", formProps);
        },

        planCreated: function (planId, eventInfo) {
            return DailyOps.eventstore.appendToStream("plans", "planCreated", eventInfo);
        },

        planRenamed: function (planId, eventInfo) {
            return DailyOps.eventstore.appendToStream("plan-" + planId, "planRenamed", eventInfo);
        },

        planRemoved: function (planId, eventInfo) {
            return DailyOps.eventstore.appendToStream("plan-" + planId, "planRemoved", eventInfo);
        },

        taskAdded: function (planId, eventInfo) {
            return DailyOps.eventstore.appendToStream("plan-" + planId, "taskAdded", eventInfo);
        },

        taskCompleted: function (planId, eventInfo) {
            return DailyOps.eventstore.appendToStream("plan-" + planId, "taskCompleted", eventInfo);
        },

        taskCompletionRevoked: function (planId, eventInfo) {
            return DailyOps.eventstore.appendToStream("plan-" + planId, "taskCompletionRevoked", eventInfo);
        }

    };
})();
