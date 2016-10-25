﻿using DailyOps.Events;
using Nuclear.Domain;
using System;
using System.Data;

namespace DailyOps.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public class Task : AggregateBase
    {
        private string title;
        private Reccurence interval;
        private Guid planId;

        private string lastCompletion;

        private IDictionary<DateTimeOffset, string> completionHistory = new SortedDictionary<DateTimeOffset, string>();

        private ReccurencePolicy reccurencePolicy;

        public Task(Guid id) : base(id)
        {
        }

        public Task(Guid id, Guid planId, string title, Reccurence taskType)
            : base(id)
        {
            AcceptChange(new TaskCreated(id, planId, title, taskType));
        }

        public DateTimeOffset? LastCompletion => this.completionHistory?.Last().Key;

        private void Apply(TaskCreated e)
        {
            Id = e.Id;
            planId = e.PlanId;
            title = e.Title;
            interval = e.Interval;
        }

        public void ChangeTitle(string newTitle)
        {
            if (this.title != null && this.title == newTitle)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No need to change the name...");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            AcceptChange(new TaskRenamed(Id, this.title));
        }


        private void Apply(TaskRenamed e)
        {
            title = e.Title;
        }


        public void MarkCompleted(string user, DateTimeOffset timestamp)
        {
            AcceptChange(new TaskMarkedCompleted(Id, user, timestamp.ToString("O"), (int) timestamp.Offset.TotalMinutes));
        }

        private void Apply(TaskMarkedCompleted e)
        {
            DateTimeOffset t;
            DateTimeOffset.TryParse(e.Timestamp, out t);

            completionHistory[t] = e.User;
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

        public Summary Summary()
        {
            return new Summary(this.title, this.interval.ToString());
        }
    }
}
