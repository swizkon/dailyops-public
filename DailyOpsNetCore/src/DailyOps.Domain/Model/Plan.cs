using System.Collections.Generic;

namespace DailyOps.Domain.Model
{
    public class Plan
    {
        public virtual long Id { get; protected set; }

        public virtual string DisplayName { get; set; }

        public virtual IList<User> Users { get; set; }

        public virtual IList<Assignment> Assignments { get; set; }
        
        public Plan(string displayName)
        {
            DisplayName = displayName;
            Users = new List<User>();
            Assignments = new List<Assignment>();
        }

        protected Plan()
        {
            
        }
    }
}
