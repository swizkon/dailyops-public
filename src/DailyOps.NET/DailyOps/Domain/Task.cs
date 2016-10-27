namespace DailyOps.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DailyOps.Events;

    using Nuclear.Domain;

    public class Task : AggregateBase
    {
        private Reccurence interval;

        private Guid planId;

        private readonly IDictionary<DateTimeOffset, string> completionHistory = new SortedDictionary<DateTimeOffset, string>();

        private ReccurencePolicy reccurencePolicy;

        public Task(Guid id)
            : base(id)
        {
        }

        public Task(TaskId id, PlanId planId, string title, Reccurence taskType)
            : base(id)
        {
            AcceptChange(new TaskCreated(id, planId, title, taskType));
        }

        public DateTimeOffset? LastCompletion
        {
            get
            {
                return (completionHistory.Count > 0) ? completionHistory.Last().Key : (DateTimeOffset?)null;
            }
        }

        public DateTime NextReapperance { get; private set; }

        public string Title { get; private set; }

        private void Apply(TaskCreated e)
        {
            Id = e.Id;
            planId = e.PlanId;
            Title = e.Title;
            interval = e.Interval;
        }

        public void ChangeTitle(string newTitle)
        {
            if (this.Title != null && this.Title == newTitle)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No need to change the name...");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            AcceptChange(new TaskRenamed(Id, newTitle));
        }

        private void Apply(TaskRenamed e)
        {
            Title = e.Title;
        }

        public void MarkCompleted(string user, DateTimeOffset timestamp)
        {
            if(LastCompletion.HasValue && LastCompletion.Value.UtcDateTime > timestamp.UtcDateTime)
                throw new InvalidOperationException("The task has a newer completion timestamp.");

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

            NextReapperance = new DateTime(lastCompletionUtc.Year, lastCompletionUtc.Month, lastCompletionUtc.Day, 8, 0, 0, 0, DateTimeKind.Utc);
        }

        public Summary Summary()
        {
            return new Summary(this.Title, this.interval.ToString());
        }
    }
}