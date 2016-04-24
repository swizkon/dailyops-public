using DailyOps.Events;
using Nuclear.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DailyOps.Domain
{
    public class Task : AggregateBase
    {
        private string title;
        private Reccurence interval;
        private Guid planId;
        private string lastCompletion;

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
            this.Id = e.Id;
            this.planId = e.PlanId;
            this.title = e.Title;
            this.interval = e.Interval;
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

            AcceptChange(new TaskRenamed(this.Id, title));
        }


        private void Apply(TaskRenamed e)
        {
            this.title = e.Title;
        }


        public void MarkCompleted(string user, DateTimeOffset timestamp)
        {
            AcceptChange(new TaskMarkedCompleted(this.Id, user, timestamp.ToString()));
        }

        private void Apply(TaskMarkedCompleted e)
        {
            this.lastCompletion = e.Timestamp;
        }
    }
}
