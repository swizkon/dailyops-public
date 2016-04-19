using DailyOps.Domain;
using Nuclear.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Commands
{

    public class CreatePersonalPlan : Command
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string Owner;

        public CreatePersonalPlan(PlanId id, string name, string description, string owner)
        {
            Id = (Guid)id;
            Name = name;
            Description = description;
            Owner = owner;
        }
    }



    public class CreateCollaborativePlan : Command
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string Owner;

        public CreateCollaborativePlan(PlanId id, string name, string description, string owner)
        {
            
            Id = (Guid) id;
            Name = name;
            Description = description;
            Owner = owner;
        }
    }


    public class CreateDistributablePlan : Command
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string Owner;

        public CreateDistributablePlan(PlanId id, string name, string description, string owner)
        {
            Id = (Guid)id;
            Name = name;
            Description = description;
            Owner = owner;
        }
    }


    /*
    public class CreateCommonPlan : Command
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string Owner;

        public CreateCommonPlan(Guid id, string name, string description, string owner)
        {
            Id = id;
            Name = name;
            Description = description;
            Owner = owner;
        }
    }
    */


    public class RenamePlan : Command
    {
        public readonly Guid Id;
        public readonly string NewName;

        public RenamePlan(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }

    public class AddCollaborator : Command
    {
        public readonly Guid PlanId;
        public readonly string Collaborator;
        public readonly string Role;

        public AddCollaborator(PlanId planId, string collaborator, string role)
        {
            PlanId = (Guid) planId;
            Collaborator = collaborator;
            Role = role;
        }
    }


}
