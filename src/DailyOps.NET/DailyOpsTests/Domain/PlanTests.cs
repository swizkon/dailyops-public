namespace DailyOpsTests.Domain
{
    using System;

    using DailyOps.Domain;

    using EventStore.ClientAPI;

    using Nuclear.Domain;
    using Nuclear.EventStore;

    using Xunit;

    public class PlanTests // : IClassFixture<Plan>
    {
        [Fact()]
        public void PlanTest()
        {
            Plan p = new Plan(Guid.NewGuid());
            p.AddCollaborator("Jonas", CollaboratorRole.Admin);
            p.AssignOwnership("Jonas");

            var switchboard = new Nuclear.Lazy.Switchboard();
            IEventStoreConnection connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));
            connection.ConnectAsync().Wait();
            // new  EventStoreConnectionException(); // = new EventStore.ClientAPI.conn
            IAggregateEventStore eventstore = new EventStoreRepository(connection, switchboard);
            // var eventstore = new Nuclear.Lazy.InMemEventStore(switchboard);
            eventstore.SaveChanges(p);

        }
    }
}