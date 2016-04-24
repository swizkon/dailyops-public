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
        private List<string> collaborators = new List<string>();

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
            if(!this.collaborators.Contains(name + ":" + role))
                AcceptChange(new PlanCollaboratorAdded(this.AggregateId, name, role));
        }

        public void AssignOwnership(string owner)
        {
            AcceptChange(new PlanCollaboratorAdded(this.AggregateId, owner, "Owner"));
        }

        private void Apply(PlanCollaboratorAdded e)
        {
            this.collaborators.Add(e.Name + ":" + e.Role);
        }
    }
}
