using System;

using DailyOps.Commands;
using DailyOps.Domain;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Nuclear.Lazy;
using Nuclear.Messaging;

namespace DailyOps.UnitTests
{
    [TestClass]
    public class TaskTests
    {
        readonly Bus bus;

        readonly PlanId planId;

        readonly TaskId _taskId;

        private Task _task;

        public TaskTests()
        {
            planId = new PlanId(Guid.NewGuid());
            _taskId = (TaskId)Guid.NewGuid();

            bus = new Switchboard();
            bus.RegisterHandler<CreateTask>((t) => { this._task = new Task(t.TaskId, t.PlanId, t.Name, t.Interval); });
        }

        [TestMethod]
        public void Expect_daily_task_to_be_daily()
        {
            var createTask = new CreateTask(planId, this._taskId, "Create unit tests", Reccurence.Daily);
            bus.Send(createTask);
            Assert.AreEqual(((Guid)this._taskId).ToString(), this._task.AggregateId.ToString());
        }
    }
}