namespace DailyOps.Events
{
    using System;

    using Nuclear.Domain;

    public class TaskAssociatedToPlan : DomainEvent
    {
        public readonly Guid TaskId;
        public readonly Guid PlanId;

        public readonly string Title;

        public TaskAssociatedToPlan(Guid taskId, Guid planId, string title)
        {
            this.TaskId = taskId;
            this.PlanId = planId;
            this.Title = title;
        }
    }
}