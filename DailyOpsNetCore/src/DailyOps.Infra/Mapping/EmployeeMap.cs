namespace DailyOps.Infra.Mapping
{
    using NHibernate;
    using FluentNHibernate.Mapping;

    using DailyOps.Domain.Model;

    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id);
            Map(x => x.FirstName)
              .Length(16)
              .Not.Nullable();

            Map(x => x.LastName)
              .Length(16)
              .Not.Nullable();
        }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            
            Map(x => x.FirstName)
              .Length(16)
              .Not.Nullable();
            
            Map(x => x.DisplayName)
              .Length(16)
              .Not.Nullable();

            Map(x => x.LastName)
              .Length(16)
              .Not.Nullable();

            References(x => x.PersonalPreferences);
        }
    }
}