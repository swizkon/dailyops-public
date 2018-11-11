namespace DailyOps.Infra
{
    using System;

    using NHibernate;
    using NHibernate.Cfg;

    using FluentNHibernate;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Mapping;

    public static class PersistenceConfiguration
    {
        public static ISessionFactory CreateSessionFactory()
        {
            // return Fluently.Configure()
            //                 .BuildSessionFactory();

            return Fluently.Configure()
                            .Database(SQLiteConfiguration.Standard.InMemory)
                            .Mappings(m =>
                                m.FluentMappings
                                .AddFromAssemblyOf<DailyOps.Infra.Mapping.UserMap>())
                            .BuildSessionFactory(); 
        }
    }
}
