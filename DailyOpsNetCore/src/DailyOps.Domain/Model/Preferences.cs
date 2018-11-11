namespace DailyOps.Domain.Model
{
    public class Preferences
    {
        public virtual int Id { get; protected set; }

        public virtual TimeInterval MorningInterval { get; set; }

        public virtual TimeInterval ForenoonInterval { get; set; }

        public virtual TimeInterval LunchInterval { get; set; }

        public virtual TimeInterval AfternoonInterval { get; set; }

        public virtual TimeInterval EveningInterval { get; set; }
        
    }
}
