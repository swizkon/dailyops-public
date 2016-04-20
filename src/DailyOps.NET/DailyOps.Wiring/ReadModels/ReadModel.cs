using FluentNHibernate.MappingModel.ClassBased;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace DailyOps.Wiring.ReadModels
{
    public abstract class ReadModel
    {
        
        Configuration cfg;

        protected ReadModel(ReadModelConnectionString connectionString)
        {
            cfg = buildConfig(connectionString);
        }

        protected ReadModel(System.Configuration.ConnectionStringSettings connectionStringSettings)
            : this((ReadModelConnectionString)connectionStringSettings.ConnectionString)
        {
        }

        private static Configuration buildConfig(ReadModelConnectionString connectionString)
        {

            var cfg = new Configuration()
               .DataBaseIntegration(db =>
               {
                   db.ConnectionString = (String)connectionString;
                   db.Dialect<MySQLDialect>();
               });

            /* Add the mapping we defined: */
            var mapper = new ModelMapper();
            mapper.AddMappings(
                    Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where<Type>(g => g.FullName.EndsWith("Map")
                        || g.IsSubclassOf(typeof(ClassMapping))
                    )
                );

            HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            cfg.AddMapping(mapping);

            return cfg;
        }



        protected IEnumerable<TModel> Query<TModel>(Func<ISession, IEnumerable<TModel>> unitOfWork)
        {
            /* Create a session and execute a query: */
            using (ISessionFactory factory = cfg.BuildSessionFactory())
            using (ISession session = factory.OpenSession())
            {
                return unitOfWork(session);
            }
        }

        protected void Transaction(Action<ISession> unitOfWork)
        {
            using (ISessionFactory factory = cfg.BuildSessionFactory())
            using (ISession session = factory.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                unitOfWork(session);
                tx.Commit();
            }
        }


        protected TModel Find<TModel>(Func<ISession, TModel> query)
        {
            /* Create a session and execute a query: */
            using (ISessionFactory factory = cfg.BuildSessionFactory())
            using (ISession session = factory.OpenSession())
            {
                return query(session);
            }
        }


        internal TModel FindById<TModel>(object id) where TModel : class
        {
            return Find<TModel>(session =>
            {
                return session.Get<TModel>(id);
            });
        }


        internal void Put<TDto>(TDto dto) where TDto : class
        {
            Transaction((session) =>
            {
                session.SaveOrUpdate(dto);
            });
        }

        internal static void CreateReadModelSchema(ReadModelConnectionString connectionString)
        {
            bool schemaCreated = false;

            if (!schemaCreated)
            {
                NHibernate.Tool.hbm2ddl.SchemaExport schema = new NHibernate.Tool.hbm2ddl.SchemaExport(buildConfig(connectionString));
                // schema.Drop(true, true);
                schema.Create(true, true);
                schemaCreated = true;
            }
        }

        internal static string ExportReadModelSchema(ReadModelConnectionString connectionString)
        {
            NHibernate.Tool.hbm2ddl.SchemaExport schema = new NHibernate.Tool.hbm2ddl.SchemaExport(buildConfig(connectionString));


            StringBuilder data = new StringBuilder();

            schema.Create((a) =>
                {
                    data.AppendLine(a);
                }, false);


            return data.ToString();

        }
    }
}
