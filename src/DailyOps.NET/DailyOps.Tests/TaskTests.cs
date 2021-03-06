﻿namespace DailyOps.Tests
{
    using System;

    using DailyOps.Domain;
    using DailyOps.Tests.Helpers;

    using NUnit.Framework;

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

            Task task = new Task((TaskId)Guid.NewGuid(), (PlanId) plan.AggregateId, title, Reccurence.Daily);

            Assert.AreEqual(title, task.Summary().Name);
        }

        [Test]
        public void When_completing_a_task_It_has_the_correct_timestamp()
        {
            var timestamp = DateTimeOffset.Now;

            Task task = new Task((TaskId)Guid.NewGuid(), (PlanId)Guid.NewGuid(), "", Reccurence.Daily);
            task.MarkCompleted("jonas", timestamp);

            Assert.AreEqual(timestamp, task.LastCompletion);
        }

        [TestCase("2016-10-25 08:32:40 +02:00", "2016-10-26 08:00:00")]
        [TestCase("2016-06-01 23:32:40 +02:00", "2016-06-02 08:00:00")]
        public void When_completing_a_daily_task_It_sets_the_correct_releaseTimestamp(string completedTimestamp, string nextRelease)
        {
            var timestamp = DateTimeOffset.Parse(completedTimestamp);
            var nextReapperance = DateTime.Parse(nextRelease);

            Task task = new Task((TaskId)Guid.NewGuid(),(PlanId) Guid.NewGuid(), "", Reccurence.Daily);
            task.MarkCompleted("jonas", timestamp);

            Assert.AreEqual(nextReapperance, task.NextReapperance);
        }

        [TestCase("2016-10-25 08:32:40 +02:00", "2016-11-25 08:32:40 +02:00", "2016-10-26 08:00:00")]
        [TestCase("2016-06-01 23:32:40 +02:00", "2016-06-21 23:32:40 +02:00", "2016-06-02 08:00:00")]
        public void When_revoking_completion_for_a_daily_task_It_sets_the_correct_releaseTimestamp(string firstTimestamp, string secondTimestamp, string expectedReapperance)
        {
            Task task = new Task((TaskId)Guid.NewGuid(), (PlanId)Guid.NewGuid(), "", Reccurence.Daily);

            var firstCompletion = DateTimeOffset.Parse(firstTimestamp);
            task.MarkCompleted("jonas", firstCompletion);

            var secondCompletion = DateTimeOffset.Parse(secondTimestamp);
            task.MarkCompleted("jonas", secondCompletion);

            task.RevokeCompletion("jonas", secondCompletion);

            var expectedReapperanceDateTime = DateTime.Parse(expectedReapperance);
            Assert.AreEqual(expectedReapperanceDateTime, task.NextReapperance);
        }


        [TestCase("2016-10-25 08:32:40 +02:00", "2016-09-25 08:32:40 +02:00")]
        [TestCase("2016-06-01 23:32:40 +02:00", "2016-06-01 07:32:40 +02:00")]
        [TestCase("2016-06-01 00:32:40 +02:00", "2016-06-01 01:32:40 +04:00")]
        public void When_completing_a_task_with_older_date_It_throws_exception(string firstTimestamp, string secondTimestamp)
        {
            Task task = new Task((TaskId)Guid.NewGuid(),(PlanId) Guid.NewGuid(), "", Reccurence.Daily);
            
            var firstCompletion = DateTimeOffset.Parse(firstTimestamp);
            task.MarkCompleted("jonas", firstCompletion);

            var secondCompletion = DateTimeOffset.Parse(secondTimestamp);
            
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    task.MarkCompleted("jonas", secondCompletion);
                });
        }

        [TestCase("2016-10-19 08:32:40 +02:00", "2016-10-24 08:00:00")]
        [TestCase("2016-10-24 08:32:40 +02:00", "2016-10-31 08:00:00")]
        [TestCase("2016-10-25 08:32:40 +02:00", "2016-10-31 08:00:00")]
        [TestCase("2016-10-26 08:32:40 +02:00", "2016-10-31 08:00:00")]
        [TestCase("2016-10-27 08:32:40 +02:00", "2016-10-31 08:00:00")]
        [TestCase("2016-10-28 08:32:40 +02:00", "2016-10-31 08:00:00")]
        [TestCase("2016-10-29 08:32:40 +02:00", "2016-10-31 08:00:00")]
        [TestCase("2016-10-30 08:32:40 +02:00", "2016-10-31 08:00:00")]
        public void When_completing_a_weekly_task_It_sets_the_correct_releaseTimestamp(string completedTimestamp, string nextRelease)
        {
            var timestamp = DateTimeOffset.Parse(completedTimestamp);
            var nextReapperance = DateTime.Parse(nextRelease);

            Task task = new Task((TaskId)Guid.NewGuid(),(PlanId) Guid.NewGuid(), "", Reccurence.Weekly);
            task.MarkCompleted("jonas", timestamp);

            Assert.AreEqual(nextReapperance, task.NextReapperance);
        }


        [TestCase("2016-09-19 08:32:40 +02:00", "2016-10-01 08:00:00")]
        [TestCase("2016-10-19 08:32:40 +02:00", "2016-11-01 08:00:00")]
        public void When_completing_a_monthly_task_It_sets_the_correct_releaseTimestamp(string completedTimestamp, string nextRelease)
        {
            var timestamp = DateTimeOffset.Parse(completedTimestamp);
            var nextReapperance = DateTime.Parse(nextRelease);

            Task task = new Task((TaskId)Guid.NewGuid(),(PlanId) Guid.NewGuid(), "", Reccurence.Monthly);
            task.MarkCompleted("jonas", timestamp);

            Assert.AreEqual(nextReapperance, task.NextReapperance);
        }
    }
}
