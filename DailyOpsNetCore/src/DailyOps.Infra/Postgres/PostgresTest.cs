namespace DailyOps.Infra
{

    using NHibernate;
    using FluentNHibernate.Mapping;

    public class PostgresTest
    {
        public PostgresTest()
        {
            
        }
    }

    public class Employee
    {
        public virtual int Id { get; protected set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

    }
}
