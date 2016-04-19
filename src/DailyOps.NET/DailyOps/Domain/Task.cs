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
        private TaskType type;
        private Guid planId;

        public Task(Guid id, Guid planId, string title)
            : base(id)
        {
            AcceptChange(new TaskCreated(id, planId, title));
        }

        public Task(Guid id) : base(id)
        {
        }


        internal void ChangeTitle(string title)
        {
            if (this.title != null
                && this.title == title)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No need to change the name...");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            AcceptChange(new TaskRenamed(this.Id, title));
        }


        private void Apply(TaskCreated e)
        {
            this.Id = e.Id;
            this.planId = e.PlanId;
            this.title = e.Title;
        }

        private void Apply(TaskRenamed e)
        {
            this.title = e.Title;
        }

    }
}
