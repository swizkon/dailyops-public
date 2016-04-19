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

        public TaskCreated(Guid id, Guid planId, string title)
        {
            Id = id;
            PlanId = planId;
            Title = title;
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

    public class TaskCompleted : DomainEvent
    {
        public readonly Guid Id;
        public readonly string User;

        public TaskCompleted(Guid id, string user)
        {
            Id = id;
            User = user;
        }
    }
}
