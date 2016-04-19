using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Domain
{
    public sealed class PlanId
    {
        private Guid id;

        public PlanId()
            : this(Guid.NewGuid())
        {
        }

        public PlanId(Guid id)
        {
            this.id = id;
        }

        public static explicit operator PlanId(Guid id)
        {
            return new PlanId(id);
        }

        public static explicit operator Guid(PlanId planId)
        {
            return planId.id;
        }

    }
}
