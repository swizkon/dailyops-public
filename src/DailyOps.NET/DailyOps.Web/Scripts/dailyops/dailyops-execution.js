/// <reference path="../knockout-2.2.0.debug.js" />


(function () { // Wrap in function to prevent accidental globals
    if (location.protocol != "data:") {
        $(window).bind('hashchange', function () {
            window.parent.handleChildIframeUrlChange(location.hash)
        });
    }

    function TasksViewModel() {
        // Data
        var self = this;

        self.remainingToday = [];

        self.completedToday = [];

        self.markCompleted = function (task) {
            var browserDate = new Date();
            $.post('/execution/completedTasks',
                {
                    'task': task.TaskId,
                    'ts': browserDate.toString(),
                    'localTimestamp': browserDate.toISOString(),
                    'timeOffset': browserDate.getTimezoneOffset()
                },
                function (data) {
                // self.remainingToday = data;
                // self.completedTasks(self.completedToday);
                // self.remainingTasks(self.remainingToday);
                }
            );
            // console.log(task);
            self.remainingToday = self.remainingToday.filter(function (t) {
                return t.TaskId !== task.TaskId;
            });
            self.completedToday.push(task);
            self.completedTasks(self.completedToday);
            self.remainingTasks(self.remainingToday);
        };

        self.revokeCompleted = function (task) {
            // console.log(task);
            self.completedToday = self.completedToday.filter(function (t) {
                return t.TaskId !== task.TaskId;
            });
            self.remainingToday.push(task);
            self.completedTasks(self.completedToday);
            self.remainingTasks(self.remainingToday);
        };

        //
        // Task management
        self.loadTasks = function (filter, obserableTarget) {
            // $.get('/execution/' + filter, { 'context': filter }, obserableTarget);
            $.get('/execution/' + filter, { 'context': filter }, function (data) {
                self.remainingToday = data;
                self.completedTasks(self.completedToday);
                self.remainingTasks(self.remainingToday);
            });
            // self.allTasks = ;
        };

        self.remainingTasks = ko.observable();
        self.completedTasks = ko.observable([]);
        self.loadTasks('allTasks', self.remainingTasks);
    };

    ko.applyBindings(new TasksViewModel());
})();