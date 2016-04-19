using Nuclear.Domain;
using Nuclear.EventSourcing;
using Nuclear.EventSourcing.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DailyOps.Wiring
{
    internal class RepositoryFactory : IDisposable
    {
        private Nuclear.Messaging.Bus bus;
        private StructureMap.IContainer container;

        public RepositoryFactory(Nuclear.Messaging.Bus bus, StructureMap.IContainer container)
        {
            // TODO: Complete member initialization
            this.bus = bus;
            this.container = container;
        }

        private IAggregateEventStore EventStore()
        {
            return new MySqlEventSource(container.GetInstance<MySqlConnectionString>(), bus);
        }

        public AggregateRepository<T> Build<T>()
            where T : class, Aggregate
        {
            return new EventSourcedAggregateRepository<T>(EventStore());
        }

        public void Dispose()
        {
        }
    }
}
