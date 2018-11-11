using System;

namespace DailyOps.Domain.Model
{
    public class TimeInterval
    {
        public virtual DateTime Starts { get; set; }

        public virtual DateTime Ends { get; set; }
    }
}
