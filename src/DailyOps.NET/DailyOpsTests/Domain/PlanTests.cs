using Xunit;
using DailyOps.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.DomainTests
{
    public class PlanTests // : IClassFixture<Plan>
    {
        [Fact()]
        public void PlanTest()
        {
            Plan p = new Plan(Guid.NewGuid());
            p.AddCollaborator("Jonas", CollaboratorRole.Admin);
            p.AssignOwnership("Jonas");

            var switchboard = new Nuclear.Lazy.Switchboard();

            var eventstore = new Nuclear.Lazy.InMemEventStore(switchboard);
            eventstore.SaveChanges(p);

        }
    }
}