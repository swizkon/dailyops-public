using DailyOps.Events;
using Nuclear.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Domain
{
    public class Plan : AggregateBase
    {
        public Plan(Guid id, string name, string description, string owner, PlanType typeOfPlan)
            : this(id)
        {
            if (typeOfPlan == PlanType.Personal)
            {
                AcceptChange(new PersonalPlanCreated(id, name, description, owner));
            }
            else if (typeOfPlan == PlanType.Distributable)
            {
                AcceptChange(new DistributablePlanCreated(id, name, description, owner));
            }
            else if (typeOfPlan == PlanType.Collaborative)
            {
                AcceptChange(new CollaborativePlanCreated(id, name, description, owner));
            }
        }
        

        public Plan(Guid id) : base(id)
        {
        }

        public void AddCollaborator(string name, string role)
        {
            AcceptChange(new PlanCollaboratorAdded(this.AggregateId, name, role));
        }

        public void AssignOwnership(string owner)
        {
            AcceptChange(new PlanCollaboratorAdded(this.AggregateId, owner, "Admin"));
        }
    }
}
