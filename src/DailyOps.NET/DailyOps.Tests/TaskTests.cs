namespace DailyOps.Tests
{
    using System;

    using DailyOps.Domain;
    using DailyOps.Tests.Helpers;

    using NUnit.Framework;
    using NUnit.Framework.Internal;

    using Ploeh.AutoFixture;

    [TestFixture]
    public class TaskTests
    {
        Fixture fixture = new Fixture();

        [Test]
        public void When_Creating_a_task_It_has_the_correct_summary()
        {
            Plan plan = this.fixture.ConstructPlan();
            string title = this.fixture.Create<string>();

            Task task = new Task(Guid.NewGuid(), plan.AggregateId, title, Reccurence.Daily);

            Assert.AreEqual(title, task.Summary().Name);
        }

        [Test]
        public void When_completing_a_task_It_has_the_correct_timestamp()
        {
            var timestamp = DateTimeOffset.Now;

            Task task = new Task(Guid.NewGuid(), Guid.NewGuid(), "", Reccurence.Daily);
            task.MarkCompleted("jonas", timestamp);

            Assert.AreEqual(timestamp, task.LastCompletion);
        }
    }
}
