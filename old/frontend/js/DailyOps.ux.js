DailyOps = window['DailyOps'] || {};

DailyOps.ux = (function () {

    return {

        navigateToPanel: function (panelId) {
            $("body section div.container").not(panelId).hide();
            $(panelId).show();
        },

        bindNavigation: function () {
            $(".navigates-to-panel").click(function (e) {
                var sender = $(this);
                DailyOps.ux.navigateToPanel(sender.attr("href"));
                e.preventDefault();
            });
        },

        bindStatics: function () {
            $("a.sign-out").click(function () {
                DailyOps.users.signOut("").then(function () {
                    document.location.reload();
                });
            });
        },

        bindUI: function () {
            console.log("bindUI();");
            DailyOps.ux.bindStatics();
            DailyOps.ux.bindNavigation();
        }
    };
})();


// Binds some known events on load...
$(document).ready(function () {



    $("#plans-panel").on("rendered", function (evt) {
        // This should 
        $("#plans-panel a.plan-link").click(function () {
            var sender = $(this);
            $("input.current-plan-id").val(sender.data("planid"));
            $("h2.current-plan-name").html(sender.html());
            // load tasks...
            loadTasks(sender.data("planid"), function () { });
        });
        DailyOps.ux.bindStatics();
        DailyOps.ux.bindNavigation();
    });

    $("#plans-panel").on("disposed", function (evt) {
        // A chance to clean up...
    });

    $("#plan-task-list").on("rendered", function (evt) {
        // This should 
        console.log("Hook up the task-link");
        $("#plan-task-list a.task-link").click(function () {
            console.log("task-link was clicked");
            var sender = $(this);
            $("input.current-task-id").val(sender.data("taskid"));
            $("h2.current-task-title").html(sender.html());

        });
        DailyOps.ux.bindStatics();
        DailyOps.ux.bindNavigation();
    });

    // TODO Refactor this callback chain..
    $("nav").load("global-nav.html #global-nav", function () {
        DailyOps.ux.bindUI();
    });
    DailyOps.ux.bindStatics();
    DailyOps.ux.bindNavigation();


});
