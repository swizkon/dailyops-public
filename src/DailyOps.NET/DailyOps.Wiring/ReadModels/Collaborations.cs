﻿using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Wiring.ReadModels
{
    public class CollaborationRepo : ReadModel
    {
        public CollaborationRepo(ReadModelConnectionString connectionString)
            : base(connectionString)
        {

        }


        public IEnumerable<CollaboratorDto> ByPlanId(Guid planId)
        {
            return base.Query<CollaboratorDto>((session) =>
            {
                return session.QueryOver<CollaboratorDto>().Where(t => t.PlanId == planId).List();
            });
        }

        /*
        internal void Put(CollaboratorDto collaboratorDto)
        {
            base.Transaction((session) =>
            {
                session.SaveOrUpdate(collaboratorDto);
            });
        }
        */

        public IEnumerable<Guid> AvailablePlansForUser(IPrincipal principal)
        {
            return base.Query<CollaboratorDto>((session) =>
            {
                return session.QueryOver<CollaboratorDto>().Where(t => t.Username == principal.Identity.Name).List();
            })
            .ToList()
            .ConvertAll<Guid>(
                c =>
                {
                    return c.PlanId;
                }
                )
                .Distinct();
        }
    }


    internal class CollaborationInviteMap : ClassMapping<CollaborationInviteDto>
    {
        public CollaborationInviteMap()
        {
            this.Table("dailyops_collaboration_invites");
            this.Id(p => p.InviteId);
            this.Property(p => p.PlanId);
            this.Property(p => p.Title);
            this.Property(p => p.Description);
            this.Property(p => p.Accepted);
        }
    }

    public class CollaborationInviteDto
    {
        public virtual Guid PlanId { get; set; }

        public virtual Guid InviteId { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Accepted { get; set; }
    }


    internal class CollaboratorsMap : ClassMapping<CollaboratorDto>
    {
        public CollaboratorsMap()
        {
            this.Table("dailyops_collaboration_collaborators");
            this.Id(p => p.CollaboratorId);
            this.Property(p => p.PlanId);
            this.Property(p => p.DisplayName);
            this.Property(p => p.Username);
            this.Property(p => p.Role);
        }
    }

    public class CollaboratorDto
    {
        public virtual Guid CollaboratorId { get; set; }
        public virtual Guid PlanId { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Username { get; set; }
        public virtual string Role { get; set; }
    }
}
