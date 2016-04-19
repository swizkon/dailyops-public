
DailyOps = window['DailyOps'] || {};

function generateUUID() {
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = (d + Math.random()*16)%16 | 0;
        d = Math.floor(d/16);
        return (c=='x' ? r : (r&0x3|0x8)).toString(16);
    });
    return uuid;
};

DailyOps.eventstore = {

  	host: "http://localhost",
	  port: "2113",
  	
	  appendToStream: function(streamName, eventType, eventBody){ // , callback
		  var eventData = [
			  {
			      "eventId": generateUUID(),
			      "eventType": eventType,
			      "data": eventBody
			  }
		  ];
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
