/// <reference path="../knockout-2.2.0.debug.js" />


(function () { // Wrap in function to prevent accidental globals
    if (location.protocol != "data:") {
        $(window).bind('hashchange', function () {
            // window.parent.handleChildIframeUrlChange(location.hash)
        });
    }

    function PlansViewModel() {
        // Data
        var self = this;

        self.plans = ko.observable();
        self.chosenPlanId = ko.observable();
        self.chosenPlanData = ko.observable(null);
        self.createPlanModel = ko.observable({planName:'The name', planType: 'SharedPlan'});

        //
        // Task management
        self.loadPlans = function (filter, obserableTarget) {
            // self.chosenFolderId(folder);
            self.chosenPlanData(null);
            $.get('/planning/plans', { 'context': filter }, obserableTarget);
        };
        // self.loadPlans('plans', self.plans);

        self.createPlan = function (form) {
        };

        self.createTaskToPlan = function (form) {
        };

        // Behaviours    
        self.planIndex = function () { location.hash = '' };
        self.goToPlan = function (plan) { location.hash = plan.PlanId };
        self.goToMail = function (mail) { location.hash = mail.folder + '/' + mail.id };

        // Client-side routes    
        Sammy(function () {
            this.get('#:planId', function () {
                self.chosenPlanId(this.params.planId);
                // self.chosenPlanData(null);
                $.get("/planning/plandetails", { planId: this.params.planId }, self.chosenPlanData);
                self.plans(null);
            });

            this.post('/planning/plans', function (obj, callback) {
                $.post("/planning/plans", $(obj.target).serialize(),
                    function (result) {
                        document.location.hash = result.Id;
                    }
                );
            });

            this.post('/planning/tasks', function (obj, callback) {
                $.post("/planning/tasks", $(obj.target).serialize(),
                    function (result) {
                        document.location.hash = result.PlanId;
                        $.get("/planning/plandetails", { planId: result.PlanId }, self.chosenPlanData);
                    }
                );
            });

            this.post('/planning/collaborators', function (obj, callback) {
                $.post("/planning/collaborators", $(obj.target).serialize(),
                    function (result) {
                        document.location.hash = result.PlanId;
                        $.get("/planning/plandetails", { planId: result.PlanId }, self.chosenPlanData);
                    }
                );
            });


            this.get('/planning', function () {
                self.loadPlans('plans', self.plans);
            });

        }).run();
    };

    ko.applyBindings(new PlansViewModel());
})();




