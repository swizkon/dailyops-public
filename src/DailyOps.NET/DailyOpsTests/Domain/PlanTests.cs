namespace DailyOpsTests.Domain
{
    using System;
    using System.Linq;
    using System.Net;

    using DailyOps.Domain;

    using EventStore.ClientAPI;

    using Nuclear.Domain;
    using Nuclear.EventSourcing;
    using Nuclear.EventStore;

    using Xunit;

    public class PlanTests
    {
        [Fact()]
        public void PlanTest()
        {
            // fb36789e-5cf2-4c8a-8d48-e2ee0b7727a9
            Guid planGuid = new Guid("fb36789e-5cf2-4c8a-8d48-e2ee0b7727a9");

            WithAggregate<Plan>(planGuid,
                (plan) =>
                    {
                        plan.AddCollaborator("Daddy Mac", CollaboratorRole.Admin);
                    });

        }

        void WithEventstore(Action<IAggregateEventStore> work)
        {
            var switchboard = new Nuclear.Lazy.Switchboard();
            IEventStoreConnection connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            connection.ConnectAsync().Wait();
            IAggregateEventStore eventstore = new EventStoreRepository(connection, switchboard);
            work(eventstore);
            connection.Close();
        }

        void WithRepository<TAggregate>(Action<AggregateRepository<TAggregate>> work)
            where TAggregate : class, Aggregate
        {
            WithEventstore(
                (eventstore) =>
                    {
                        AggregateRepository<TAggregate> repository =
                            new EventSourcedAggregateRepository<TAggregate>(eventstore);
                        work(repository);
                    });
        }

        void WithAggregate<TAggregate>(Guid aggregateId, Action<TAggregate> work)
            where TAggregate : class, Aggregate
        {
            WithRepository<TAggregate>(
                (repository) =>
                    {
                        var aggregate = repository.GetById(aggregateId);
                        work(aggregate);
                        repository.Save(aggregate);
                });
        }
    }
}