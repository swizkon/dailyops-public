namespace DailyOps.Tests.Helpers
{
    using System;

    using DailyOps.Domain;

    using Ploeh.AutoFixture;

    public static class FixtureExtensions
    {
        public static Plan ConstructPlan(
            this Fixture fixture,
            Guid? planId = null,
            string name = null,
            string desc = null,
            string owner = null,
            PlanType? planType = null)
        {
            return new Plan(
                id: planId ?? fixture.Create<Guid>(), 
                name: name ?? fixture.Create<string>(), 
                description: desc ?? fixture.Create<string>(),
                owner: owner ?? fixture.Create<string>(), typeOfPlan: planType ?? fixture.Create<PlanType>());
        }
    }
}
