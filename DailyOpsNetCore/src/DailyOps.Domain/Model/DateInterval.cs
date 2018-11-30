using System;

namespace DailyOps.Domain.Model
{
    public class DateWindow
    {
        public virtual DateTime Reapperance { get; set; }

        public virtual DateTime ClosingDate { get; set; }
        
        protected DateWindow()
        {
            
        }

        public DateWindow(DateTime reapperance, DateTime closingDate)
        {
            Reapperance = reapperance;
            ClosingDate = closingDate;
        }

        public override string ToString()
        {
            return Reapperance + "/" + ClosingDate;
        }
    }
}
