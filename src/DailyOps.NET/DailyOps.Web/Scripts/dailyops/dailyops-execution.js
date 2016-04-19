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
        self.folders = ['Completed today', 'Today', 'Upcoming'];
        self.chosenFolderId = ko.observable();
        self.chosenFolderData = ko.observable();


        self.goToFolder = function (folder) {
            self.chosenFolderId(folder);
            // alert(folder);
            $.get('/mail', { folder: folder }, self.chosenFolderData);
        };
        // self.goToFolder('Inbox');

        //
        // Task management
        self.loadTasks = function (filter, obserableTarget) {
            // self.chosenFolderId(folder);
            $.get('/execution/' + filter, { 'context': filter }, obserableTarget);
        };

        self.completedTasks = ko.observable();
        self.loadTasks('completedTasks', self.completedTasks);

        self.remainingTasks = ko.observable();
        self.loadTasks('remainingTasks', self.remainingTasks);
    };

    ko.applyBindings(new TasksViewModel());
})();