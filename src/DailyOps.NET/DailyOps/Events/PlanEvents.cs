namespace DailyOps.Events
{
    using System;

    using Nuclear.Domain;

    public class PersonalPlanCreated : PlanCreated
    {
        public PersonalPlanCreated(Guid id, string name, string description, string owner)
            : base(id, name, description, owner)
        {

        }
    }

    public class DistributablePlanCreated : PlanCreated
    {
        public DistributablePlanCreated(Guid id, string name, string description, string owner)
            : base(id, name, description, owner)
        {
        }
    }

    public class CollaborativePlanCreated : PlanCreated
    {
        public CollaborativePlanCreated(Guid id, string name, string description, string owner)
            : base(id, name, description, owner)
        {
        }
    }

    public abstract class PlanCreated : DomainEvent
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string Owner;

        protected PlanCreated(Guid id, string name, string description, string owner)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Owner = owner;
        }
    }

    /*
    public class TaskAddedToPlan : DomainEvent
    {
        public readonly Guid PlanId;
        public readonly Guid TaskId;
        public readonly string Name;

        public TaskAddedToPlan(Guid planId, Guid taskId, string name)
        {
            PlanId = planId;
            TaskId = taskId;
            Name = name;
        }
    }
    */

    public class PlanRenamed : DomainEvent
    {
        public readonly Guid Id;
        public readonly string Name;

        public PlanRenamed(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class PlanMadePublic : DomainEvent
    {
        public readonly Guid Id;

        public PlanMadePublic(Guid id)
        {
            Id = id;
        }
    }


    public class PlanMadePrivate : DomainEvent
    {
        public readonly Guid Id;

        public PlanMadePrivate(Guid id)
        {
            Id = id;
        }
    }


    public class PlanCollaboratorAdded : DomainEvent
    {
        public readonly Guid PlanId;
        public readonly string Name;
        public readonly string Role;

        public PlanCollaboratorAdded(Guid planId, string name, string role)
        {
            PlanId = planId;
            Name = name;
            Role = role;
        }
    }

}
