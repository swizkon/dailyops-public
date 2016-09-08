using DailyOps.Events;
using Nuclear.Domain;
using System;

namespace DailyOps.Domain
{
    public class Task : AggregateBase
    {
        private string title;
        private Reccurence interval;
        private Guid planId;
        private string lastCompletion;

        private ReccurencePolicy reccurencePolicy;

        public Task(Guid id) : base(id)
        {
        }

        public Task(Guid id, Guid planId, string title, Reccurence taskType)
            : base(id)
        {
            AcceptChange(new TaskCreated(id, planId, title, taskType));
        }


        private void Apply(TaskCreated e)
        {
            Id = e.Id;
            planId = e.PlanId;
            title = e.Title;
            interval = e.Interval;
        }

        internal void ChangeTitle(string title)
        {
            if (this.title != null && this.title == title)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No need to change the name...");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            AcceptChange(new TaskRenamed(Id, title));
        }


        private void Apply(TaskRenamed e)
        {
            title = e.Title;
        }


        public void MarkCompleted(string user, DateTimeOffset timestamp)
        {
            AcceptChange(new TaskMarkedCompleted(Id, user, timestamp.ToString()));
        }

        private void Apply(TaskMarkedCompleted e)
        {
            lastCompletion = e.Timestamp;
        }

        public void RevokeCompletion(string user, DateTimeOffset timestamp)
        {
            AcceptChange(new TaskCompletionRevoked(Id, user, timestamp.ToString()));
        }

        private void Apply(TaskCompletionRevoked e)
        {
            lastCompletion = e.Timestamp;
        }
    }
}
