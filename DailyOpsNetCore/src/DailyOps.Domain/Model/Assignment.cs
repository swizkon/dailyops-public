using System;
using System.Collections.Generic;

namespace DailyOps.Domain.Model
{
    public class Assignment
    {
        public virtual int Id { get; protected set; }

        public virtual string Summary { get; set; }

        public virtual Reccurence Interval { get; protected set; }

        private readonly IDictionary<DateTimeOffset, string> completionHistory = new SortedDictionary<DateTimeOffset, string>();


        public Assignment(string summary, Reccurence interval)
        {
            Summary = summary;
            Interval = interval;
        }

        protected Assignment()
        {

        }
    }
}