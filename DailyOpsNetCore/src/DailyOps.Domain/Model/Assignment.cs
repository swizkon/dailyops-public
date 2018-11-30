using System;
using System.Collections.Generic;

namespace DailyOps.Domain.Model
{
    public class Assignment
    {
        public virtual int Id { get; protected set; }

        public virtual string Summary { get; set; }

        public virtual Reccurence Interval { get; protected set; }

        public virtual DateWindow DateInterval { get; protected set; }

        private readonly IDictionary<DateTimeOffset, string> completionHistory = new SortedDictionary<DateTimeOffset, string>();

        public Assignment(string summary, Reccurence interval)
        {
            Summary = summary;
            Interval = interval;
            DateInterval = new DateWindow(DateTime.Now, DateTime.Now.AddDays(3));
        }

        protected Assignment()
        {

        }

        public virtual void MarkAsCompleted(DateTime completionDate)
        {
            RecalculateDates(completionDate);
        }

        private void RecalculateDates(DateTime completionDate)
        {
            // DateInterval.Reapperance = completionDate;

            if (Interval == Reccurence.Daily)
            {
                DateInterval.Reapperance = new DateTime(completionDate.Year, completionDate.Month, completionDate.Day).AddDays(1);
                DateInterval.ClosingDate = DateInterval.Reapperance.AddDays(1).AddMilliseconds(-1);
                return;
            }

            if (Interval == Reccurence.Monthly)
            {
                DateInterval.Reapperance = new DateTime(completionDate.Year, completionDate.Month, completionDate.Day).AddMonths(1);
                DateInterval.ClosingDate = DateInterval.Reapperance.AddMonths(1).AddMilliseconds(-1);
                return;
            }

            if (Interval == Reccurence.Annual)
            {
                DateInterval.Reapperance = new DateTime(completionDate.Year + 1, 1, 1);
                DateInterval.ClosingDate = DateInterval.Reapperance.AddYears(1).AddMilliseconds(-1);
                return;
            }

        }
    }
}