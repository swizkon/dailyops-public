using System;
using System.Globalization;

using DailyOps.Events;

using Nuclear.Domain;

namespace DailyOps.Domain
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    public class Task : AggregateBase
    {
        private string title;

        private Reccurence interval;

        private Guid planId;

        private IDictionary<DateTimeOffset, string> completionHistory = new SortedDictionary<DateTimeOffset, string>();

        private ReccurencePolicy reccurencePolicy;

        public Task(Guid id)
            : base(id)
        {
        }

        public Task(Guid id, Guid planId, string title, Reccurence taskType)
            : base(id)
        {
            AcceptChange(new TaskCreated(id, planId, title, taskType));
        }

        public DateTimeOffset? LastCompletion => this.completionHistory?.Last().Key;

        public DateTime NextReapperance { get; private set; }

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
            AcceptChange(new TaskMarkedCompleted(Id, user, timestamp.ToString("O"), (int)timestamp.Offset.TotalMinutes));
        }

        private void Apply(TaskMarkedCompleted e)
        {
            DateTimeOffset t;
            DateTimeOffset.TryParse(e.Timestamp, out t);

            completionHistory[t] = e.User;
            calculateNextReapperance();
        }

        public void RevokeCompletion(string user, DateTimeOffset timestamp)
        {
            if (LastCompletion != timestamp) return;
            AcceptChange(new TaskCompletionRevoked(Id, user, timestamp.ToString("O")));
        }

        private void Apply(TaskCompletionRevoked e)
        {
            DateTimeOffset t;
            DateTimeOffset.TryParse(e.Timestamp, out t);
            this.completionHistory.Remove(t);
            calculateNextReapperance();
        }

        private void calculateNextReapperance()
        {
            if (!LastCompletion.HasValue)
            {
                this.NextReapperance = new DateTime(1970, 1, 1);
                return;
            }

            DateTimeOffset lastCompletion = LastCompletion.Value;
            var lastCompletionUtc = lastCompletion.UtcDateTime;

            switch (interval)
            {
                case Reccurence.Daily:
                    lastCompletionUtc = lastCompletionUtc.AddDays(1);
                    break;

                case Reccurence.Weekly:
                    lastCompletionUtc = lastCompletionUtc.AddDays(1);
                    while (lastCompletionUtc.DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                        lastCompletionUtc = lastCompletionUtc.AddDays(1);
                    break;

                case Reccurence.Monthly:
                    lastCompletionUtc = lastCompletionUtc.AddDays(1);
                    while (lastCompletionUtc.Day != 1)
                        lastCompletionUtc = lastCompletionUtc.AddDays(1);
                    break;

                default:
                    this.NextReapperance = new DateTime(1970, 1, 1);
                    break;
            }

            NextReapperance = new DateTime(
                                  lastCompletionUtc.Year,
                                  lastCompletionUtc.Month,
                                  lastCompletionUtc.Day,
                                  8,
                                  0,
                                  0,
                                  0,
                                  DateTimeKind.Utc);
        }

        public Summary Summary()
        {
            return new Summary(this.title, this.interval.ToString());
        }
    }
}