using DailyOps.Domain;
using Nuclear.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Commands
{

    public class CreateTask : Command
    {
        public readonly Guid PlanId;
        public readonly Guid TaskId;
        public readonly string Name;
        public readonly Reccurence Interval;

        public CreateTask(PlanId planId, TaskId taskId, string name, Reccurence interval)
        {
            PlanId = (Guid) planId;
            TaskId = (Guid) taskId;
            Name = name;
            Interval = interval;
        }
    }


    public class MarkTaskCompleted : Command
    {
        public readonly Guid TaskId;
        public readonly string Timestamp;
        public readonly string CompletedBy;

        public MarkTaskCompleted(TaskId taskId, string completedBy, DateTimeOffset timestamp)
        {
            TaskId = (Guid)taskId;
            CompletedBy = completedBy;
            Timestamp = timestamp.ToString();
        }
    }


    public class RevokeTaskCompletion : Command
    {
        public readonly string RevocationTimestamp;
        public readonly Guid TaskId;
        public readonly string RevokedBy;

        public RevokeTaskCompletion(TaskId taskId, string revokedBy, string revocationTimestamp)
        {
            TaskId = (Guid)taskId;
            RevokedBy = revokedBy;
            RevocationTimestamp = revocationTimestamp;
        }
    }
}
