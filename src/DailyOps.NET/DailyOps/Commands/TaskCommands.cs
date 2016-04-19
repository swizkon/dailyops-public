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
        public readonly TaskType TaskType;

        public CreateTask(PlanId planId, TaskId taskId, string name, TaskType type)
        {
            PlanId = (Guid) planId;
            TaskId = (Guid) taskId;
            Name = name;
            TaskType = type;
        }
    }


    public class MarkTaskCompleted : Command
    {
        public readonly DateTimeOffset Timestamp;
        public readonly Guid TaskId;
        public readonly string CompletedBy;

        public MarkTaskCompleted(Guid taskId, string completedBy, DateTimeOffset timestamp)
        {
            TaskId = taskId;
            CompletedBy = completedBy;
            Timestamp = timestamp;
        }
    }

}
