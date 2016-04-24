using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyOps.Domain;
using DailyOps.Commands;
using Nuclear.Messaging;
using Nuclear.Lazy;

namespace DailyOps.UnitTests
{
    [TestClass]
    public class TaskTests
    {
        Bus bus;
        PlanId planId;
        TaskId taskId;


        Task task;

        public TaskTests()
        {
            planId = new PlanId(Guid.NewGuid());
            taskId = new TaskId(Guid.NewGuid());

            bus = new Switchboard();
            bus.RegisterHandler<CreateTask>((t) => {
                this.task = new Task(t.TaskId, t.PlanId, t.Name, t.Interval);
            });
        }



        [TestMethod]
        public void Expect_daily_task_to_be_daily()
        {
            CreateTask createTask = new CreateTask(planId, taskId, "Create unit tests", Reccurence.Daily);
            bus.Send(createTask);
            Assert.AreEqual(((Guid)taskId).ToString(), task.AggregateId.ToString());
        }
    }
}
