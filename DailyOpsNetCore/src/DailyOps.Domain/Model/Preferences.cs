using System;

namespace DailyOps.Domain.Model
{
    public class Preferences
    {
        public virtual int Id { get; protected set; }

        public virtual TimeInterval MorningInterval { get; set; }

        public virtual TimeInterval ForenoonInterval { get; set; }

        public virtual TimeInterval AfternoonInterval { get; set; }

        public virtual TimeInterval EveningInterval { get; set; }

        public Preferences()
        {
            MorningInterval = FromHours(06, 09);
            ForenoonInterval = FromHours(09, 12);
            AfternoonInterval = FromHours(12, 18);
            EveningInterval = FromHours(18, 22);
        }

        private TimeInterval FromHours(double from, double to)
        {
            return new TimeInterval(TimeSpan.FromHours(from), TimeSpan.FromHours(to));
        }
    }
}
