using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DailyOps.Domain;

namespace DailyOps.UnitTests
{
    [TestClass]
    public class PlanTests
    {

        Plan plan;

        public PlanTests()
        {
            plan = new Plan(Guid.NewGuid(), "My plan", "The description", "jonas", PlanType.Collaborative);
        }

        [TestMethod]
        public void A_owner_should_be_assignable()
        {
            this.plan.AssignOwnership("jonas");

            // Assert.AreEqual()
        }
    }
}
