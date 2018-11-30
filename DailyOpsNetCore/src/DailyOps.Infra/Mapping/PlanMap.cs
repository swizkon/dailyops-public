namespace DailyOps.Infra.Mapping
{
    using FluentNHibernate.Mapping;

    using DailyOps.Domain.Model;

    public class PlanMap : ClassMap<Plan>
    {
        public PlanMap()
        {
            Id(x => x.Id);

            Map(x => x.DisplayName)
              .Length(20)
              .Not.Nullable();

            HasManyToMany(x => x.Users)
              .Cascade.All()
              .Table("PlansUsers");

            HasMany(x => x.Assignments)
              .Cascade.All();

            // References(x => x.PersonalPreferences)
            // .Cascade.All();
        }
    }
}