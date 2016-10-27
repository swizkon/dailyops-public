namespace DailyOpsTests.Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;

    using DailyOps.Domain;

    using EventStore.ClientAPI;

    using Nuclear.Domain;
    using Nuclear.EventSourcing;
    using Nuclear.EventStore;

    using Xunit;



    public class AggregateCache<TAggregate> where TAggregate : class, Aggregate
    {
        private readonly IAggregateEventStore aggregateEventStore;

        private static ConcurrentDictionary<Guid, TAggregate> cache = new ConcurrentDictionary<Guid, TAggregate>();


        public AggregateCache(IAggregateEventStore aggregateEventStore)
        {
            this.aggregateEventStore = aggregateEventStore;
        }

        public TAggregate GetById(Guid aggregateId)
        {
            TAggregate aggregate;
            if (cache.TryGetValue(aggregateId, out aggregate))
                return aggregate;

            AggregateRepository<TAggregate> repository =
                new EventSourcedAggregateRepository<TAggregate>(aggregateEventStore);

            aggregate = repository.GetById(aggregateId);
            cache.TryAdd(aggregateId, aggregate);

            return aggregate;
        }

    }

    public class PlanTests
    {
        [Fact()]
        public void TaskCachingTest()
        {
            Guid taskGuid = new Guid("9f26571b-53e2-4254-b100-60e6c14d7e10");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            var init = timer.ElapsedMilliseconds;
            long firstLoad, secondLoad;

            WithEventstore(
                (eventstore) =>
                {
                    var cache = new AggregateCache<Task>(eventstore);
                    var task = cache.GetById(taskGuid);
                    firstLoad = timer.ElapsedMilliseconds;
                });

            WithEventstore(
                (eventstore) =>
                {
                    var cache = new AggregateCache<Task>(eventstore);
                    var task = cache.GetById(taskGuid);
                    secondLoad = timer.ElapsedMilliseconds;
                });
            
        }

        [Fact()]
        public void TaskTest()
        {
            Guid taskGuid = new Guid("9f26571b-53e2-4254-b100-60e6c14d7e10");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            var init = timer.ElapsedMilliseconds;
            long loadEvents;

            WithAggregate<Task>(taskGuid, (task) => { loadEvents = timer.ElapsedMilliseconds;});
        }

        [Fact()]
        public void PlanTest()
        {
            // fb36789e-5cf2-4c8a-8d48-e2ee0b7727a9
            Guid planGuid = new Guid("fb36789e-5cf2-4c8a-8d48-e2ee0b7727a9");

            WithAggregate<Plan>(planGuid, (plan) => { plan.AddCollaborator("Daddy Mac", CollaboratorRole.Admin); });
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

        void WithAggregate<TAggregate>(Guid aggregateId, Action<TAggregate> work) where TAggregate : class, Aggregate
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