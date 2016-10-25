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
        private readonly IDictionary<CollaboratorRole, List<string>> collaborators = new Dictionary<CollaboratorRole, List<string>>();
        private readonly IDictionary<Guid, string> tasks = new Dictionary<Guid, string>();

        private string name;

        private string description;

        private PlanType planType;

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
            this.AssignOwnership(owner);
        }
        
        public Plan(Guid id) : base(id)
        {
        }

        public void AddCollaborator(string name, CollaboratorRole role)
        {
            if (collaborators.ContainsKey(role) && collaborators[role].Contains(name))
                return;

            AcceptChange(new PlanCollaboratorAdded(this.AggregateId, name, role.ToString()));
        }

        public void AssignOwnership(string owner)
        {
            collaborators.Remove(CollaboratorRole.Owner);
            AcceptChange(new PlanCollaboratorAdded(this.AggregateId, owner, "Owner"));
        }

        private void Apply(PlanCollaboratorAdded e)
        {
            CollaboratorRole role;
            if (!Enum.TryParse(e.Role, out role)) return;

            if (!collaborators.ContainsKey(role))
                collaborators.Add(role, new List<string>());

            collaborators[role].Add(e.Name);
        }
        
        public void AssociateTask(TaskId taskId, string title)
        {
            if (tasks.ContainsKey(taskId)) return;

            AcceptChange(new TaskAssociatedToPlan(taskId: taskId, planId: AggregateId, title: title));
        }

        private void Apply(TaskAssociatedToPlan e)
        {
            tasks.Add(e.TaskId, e.Title);
        }

        private void Apply(PersonalPlanCreated personalPlanCreated)
        {
            Apply(personalPlanCreated as PlanCreated);
        }

        private void Apply(CollaborativePlanCreated collaborativePlanCreated)
        {
            Apply(collaborativePlanCreated as PlanCreated);
        }

        private void Apply(DistributablePlanCreated distributablePlanCreated)
        {
            Apply(distributablePlanCreated as PlanCreated);
        }

        private void Apply(PlanCreated planCreated)
        {
            this.name = planCreated.Name;
            this.description = planCreated.Description;
            this.planType = (PlanType) Enum.Parse(typeof(PlanType), planCreated.GetType().Name.Replace("PlanCreated", ""));
        }

        public PlanSummary Summary()
        {
            return new PlanSummary(this.name, this.description, this.planType);
        }
    }
}
