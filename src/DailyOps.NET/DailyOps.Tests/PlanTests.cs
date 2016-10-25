﻿namespace DailyOps.Tests
{
    using System;
    using System.Linq;

    using DailyOps.Domain;
    using DailyOps.Events;
    using DailyOps.Tests.Helpers;

    using NUnit.Framework;

    using Ploeh.AutoFixture;

    using Rhino.Mocks;

    [TestFixture]
    public class PlanTests
    {
        Fixture fixture = new Fixture();

        [Test]
        public void When_Creating_a_plan_It_has_the_correct_summary()
        {
            var name = this.fixture.Create<string>();
            var desc = this.fixture.Create<string>();
            string owner = this.fixture.Create<string>();
            PlanType typeOf = this.fixture.Create<PlanType>();

            Plan p = new Plan(Guid.NewGuid(), name, desc, owner, typeOf);

            Assert.AreEqual(name, p.Summary().Name);
            Assert.AreEqual(desc, p.Summary().Description);
            Assert.AreEqual(typeOf, p.Summary().PlanType);
        }


        [Repeat(100)]
        [Test]
        public void When_Creating_a_plan_It_has_the_correct_owner()
        {
            string owner = this.fixture.Create<string>();

            Plan p = this.fixture.ConstructPlan(owner: owner);

            Assert.AreEqual(owner, p.UncommittedChanges().OfType<PlanCollaboratorAdded>().Single(x => x.Role == CollaboratorRole.Owner.ToString()).Name);
        }


        [Repeat(100)]
        [Test]
        public void When_adding_collaborators_It_has_the_correct_collaborators()
        {
            Plan p = this.fixture.ConstructPlan();
            p.AddCollaborator("Jonas", CollaboratorRole.Admin);

            Assert.AreEqual("Jonas", p.UncommittedChanges().OfType<PlanCollaboratorAdded>().Single(x => x.Role == CollaboratorRole.Admin.ToString()).Name);
        }

        [Repeat(100)]
        [Test]
        public void When_adding_same_collaborator_many_times_It_only_adds_is_once()
        {
            Plan p = this.fixture.ConstructPlan();
            p.AddCollaborator("Jonas", CollaboratorRole.Admin);
            p.AddCollaborator("Jonas", CollaboratorRole.Admin);

            Assert.AreEqual(1, p.UncommittedChanges().OfType<PlanCollaboratorAdded>().Count(x => x.Name == "Jonas" && x.Role == CollaboratorRole.Admin.ToString()));
        }

        [Repeat(100)]
        [Test]
        public void When_associating_tasks_It_adds_is_once()
        {
            Task task = this.fixture.Create<Task>();
            Plan p = this.fixture.ConstructPlan();
            p.AssociateTask((TaskId)task.AggregateId, task.Title());
            p.AssociateTask((TaskId)task.AggregateId, task.Title());
            p.AssociateTask((TaskId)task.AggregateId, task.Title());

            Assert.AreEqual(1, p.UncommittedChanges().OfType<TaskAssociatedToPlan>().Count(x => x.TaskId == task.AggregateId));
        }

    }
}

