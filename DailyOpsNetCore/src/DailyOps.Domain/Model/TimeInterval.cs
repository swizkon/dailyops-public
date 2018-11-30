using System;

namespace DailyOps.Domain.Model
{
    public class TimeInterval
    {
        public virtual TimeSpan Starts { get; set; }

        public virtual TimeSpan Ends { get; set; }

        protected TimeInterval()
        {
            
        }

        public TimeInterval(TimeSpan starts, TimeSpan ends)
        {
            Starts = starts;
            Ends = ends;
        }

        public override string ToString()
        {
            return Starts + "-" + Ends;
        }
    }
}
