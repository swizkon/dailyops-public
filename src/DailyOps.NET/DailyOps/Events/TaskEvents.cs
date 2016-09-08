using DailyOps.Domain;
using Nuclear.Domain;
using Nuclear.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Events
{
    public class TaskCreated : DomainEvent
    {
        public readonly Guid Id;
        public readonly Guid PlanId;
        public readonly string Title;
        public readonly Reccurence Interval;

        public TaskCreated(Guid id, Guid planId, string title, Reccurence interval)
        {
            Id = id;
            PlanId = planId;
            Title = title;
            Interval = interval;
        }
    }

    public class TaskRenamed : DomainEvent
    {
        public readonly Guid Id;
        public readonly string Title;

        public TaskRenamed(Guid id, string title)
        {
            Id = id;
            Title = title;
        }
    }

    public class TaskMarkedCompleted : DomainEvent
    {
        public readonly Guid Id;
        public readonly string User;
        public readonly string Timestamp;

        public TaskMarkedCompleted(Guid id, string user, string timestamp)
        {
            Id = id;
            User = user;
            Timestamp = timestamp;
        }
    }

    public class TaskCompletionRevoked : DomainEvent
    {
        public readonly Guid Id;
        public readonly string User;
        public readonly string Timestamp;

        public TaskCompletionRevoked(Guid id, string user, string timestamp)
        {
            Id = id;
            User = user;
            Timestamp = timestamp;
        }
    }
}
