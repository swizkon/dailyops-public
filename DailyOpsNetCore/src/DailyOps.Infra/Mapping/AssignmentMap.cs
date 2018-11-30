namespace DailyOps.Infra.Mapping
{
    using FluentNHibernate.Mapping;

    using DailyOps.Domain.Model;

    public class AssignmentMap : ClassMap<Assignment>
    {
        public AssignmentMap()
        {
            Id(x => x.Id);

            Map(x => x.Summary)
              .Length(20)
              .Not.Nullable();
              
            Map(x => x.Interval);
            // .CustomType<Reccurence>()
            
        }
    }
}