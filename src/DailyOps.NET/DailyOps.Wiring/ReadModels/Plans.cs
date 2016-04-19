using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Wiring.ReadModels
{
    public class Plans : ReadModel
    {
        public Plans(ReadModelConnectionString connectionString)
            : base(connectionString)
        {

        }

        public IEnumerable<PlanDto> ForCurrentUser(IIdentity identity)
        {
            return base.Query<PlanDto>((session) =>
            {
                return session.QueryOver<PlanDto>().List();
            });
        }
    }


    internal class PlanMap : ClassMapping<PlanDto>
    {
        public PlanMap()
        {
            this.Table("dailyops_plans");
            this.Id(p => p.PlanId);
            this.Property(p => p.Name);
            this.Property(p => p.Description);
            this.Property(p => p.Owner);
            this.Property(p => p.PlanType);
            this.Property(p => p.NumberOfTasks);
        }
    }

    public class PlanDto
    {
        public virtual Guid PlanId { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Owner { get; set; }

        public virtual string PlanType { get; set; }

        public virtual int NumberOfTasks { get; set; }
    }
}
