using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyOps.Domain;
using System.Collections.Generic;
using Nuclear.Messaging;
// using Nuclear.Messaging; 

namespace DailyOps.UnitTests
{
    [TestClass]
    public class PlanTests
    {
        readonly Plan plan;

        public PlanTests()
        {
            plan = new Plan(Guid.NewGuid(), "My plan", "The description", "jonas", PlanType.Collaborative);
        }

        [TestMethod]
        public void A_owner_should_be_assignable()
        {
            this.plan.AssignOwnership("jonas");

            // plan.ReconstituteFromHistory(IEnumerable<Event> events);

            // Assert.AreEqual()
        }
    }
}
