﻿using System;

namespace DailyOps.Domain
{
    public sealed class PlanId
    {
        private readonly Guid id;

        public PlanId(Guid id)
        {
            this.id = id;
        }

        public static explicit operator PlanId(Guid id)
        {
            return new PlanId(id);
        }

        public static implicit operator Guid(PlanId planId)
        {
            return planId.id;
        }

        public static PlanId Create()
        {
            return new PlanId(Guid.NewGuid());
        }
    }
}
