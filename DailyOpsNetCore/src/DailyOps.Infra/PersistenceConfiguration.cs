namespace DailyOps.Infra
{
    using System;

    using NHibernate;
    using NHibernate.Cfg;

    using FluentNHibernate;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Mapping;
    using NHibernate.Tool.hbm2ddl;

    public static class PersistenceConfiguration
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                            .Database(
                                SQLiteConfiguration.Standard.UsingFile("firstProject.db")
                            )
                            
                            .Mappings(m =>
                                m.FluentMappings
                                .AddFromAssemblyOf<DailyOps.Infra.Mapping.UserMap>())
                            .ExposeConfiguration(cfg => BuildSchema(cfg, create: true))
                            .BuildSessionFactory();
        }
        
        private static void BuildSchema(Configuration config, bool create = false, bool update = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(true, true);
            }
            else
            {
                new SchemaUpdate(config).Execute(false, update);
            }
        }
    }
}
