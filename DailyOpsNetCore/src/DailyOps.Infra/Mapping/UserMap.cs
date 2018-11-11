namespace DailyOps.Infra.Mapping
{
    using NHibernate;
    using FluentNHibernate.Mapping;

    using DailyOps.Domain.Model;

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            
            Map(x => x.DisplayName)
              .Length(20)
              .Not.Nullable();
            
            Map(x => x.FirstName)
              .Length(50);

            Map(x => x.LastName)
              .Length(50);

            References(x => x.PersonalPreferences)
            .Cascade.All();
        }
    }
}