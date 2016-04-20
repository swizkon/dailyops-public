using DailyOps.Commands;
using DailyOps.Domain;
using DailyOps.Events;
using DailyOps.Wiring.ReadModels;
using Nuclear.EventSourcing.MySql;
using Nuclear.Lazy;
using Nuclear.Messaging;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace DailyOps.Wiring
{
    public class Proxy
    {

        static readonly Bus bus = ConfigureBus();

        static readonly IContainer container = ConfigureContainer();


        static Bus ConfigureBus()
        {
            var b = new Switchboard();


            //
            // Personal plan
            b.RegisterHandler<CreatePersonalPlan>((c) =>
            {
                var repository = new RepositoryFactory(bus, container).Build<Plan>();
                var plan = new Plan(c.Id, c.Name, c.Description, c.Owner, PlanType.Personal);
                plan.AssignOwnership(c.Owner);
                repository.Save(plan);
            });

            b.RegisterSubscriber<PersonalPlanCreated>((c) =>
            {
                Persist<Plans, PlanDto>(new PlanDto
                {
                    PlanId = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Owner = c.Owner,
                    NumberOfTasks = 0,
                    PlanType = PlanType.Personal.ToString()
                });
            });



            //
            // Collab plan
            b.RegisterHandler<CreateCollaborativePlan>((c) =>
            {
                var repository = new RepositoryFactory(bus, container).Build<Plan>();
                var plan = new Plan(c.Id, c.Name, c.Description, c.Owner, PlanType.Collaborative);
                plan.AssignOwnership(c.Owner);
                repository.Save(plan);
            });

            b.RegisterSubscriber<CollaborativePlanCreated>((c) =>
            {
                Persist<Plans, PlanDto>(new PlanDto
                {
                    PlanId = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Owner = c.Owner, NumberOfTasks = 0,
                    PlanType = PlanType.Collaborative.ToString()
                });
            });


            //
            // Distributable plan
            b.RegisterHandler<CreateDistributablePlan>((c) =>
            {
                var repository = new RepositoryFactory(bus, container).Build<Plan>();
                var plan = new Plan(c.Id, c.Name, c.Description, c.Owner, PlanType.Distributable);
                plan.AssignOwnership(c.Owner);

                repository.Save(plan);
            });

            b.RegisterSubscriber<DistributablePlanCreated>((c) =>
            {
                Persist<Plans, PlanDto>(new PlanDto
                {
                    PlanId = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Owner = c.Owner,
                    NumberOfTasks = 0,
                    PlanType = PlanType.Distributable.ToString()
                });

            });



            //
            // Task
            b.RegisterHandler<CreateTask>((c) =>
            {
                var repository = new RepositoryFactory(bus, container).Build<Task>();
                var task = new Task(c.TaskId, c.PlanId, c.Name);
                repository.Save(task);
            });

            b.RegisterSubscriber<TaskCreated>((c) =>
            {
                Persist<Tasks, TaskDto>(new TaskDto
                {
                    PlanId = c.PlanId,
                    Title = c.Title,
                    TaskId = c.Id
                });

                // Add number of tasks
                // REVISIT This might nde to be an scalar query.
                Mutate<Plans, PlanDto>(c.PlanId, 
                    (dto) =>
                    {
                        dto.NumberOfTasks += 1;
                    }
                );
            });


            b.RegisterHandler<AddCollaborator>((c) =>
            {
                var repository = new RepositoryFactory(bus, container).Build<Plan>();
                var plan = repository.GetById(c.PlanId);
                plan.AddCollaborator(c.Collaborator, c.Role);
                repository.Save(plan);
            });

            b.RegisterSubscriber<PlanCollaboratorAdded>((c) =>
            {
                Persist<CollaborationRepo, CollaboratorDto>(new CollaboratorDto
                {
                    PlanId = c.PlanId,
                    Username = c.Name,
                    Role = c.Role,
                    DisplayName = c.Name,
                    CollaboratorId = Guid.NewGuid()
                });
            });

            return b;
        }


        static IContainer ConfigureContainer()
        {
            var container = new Container(cfg =>
            {
                cfg.For<MySqlConnectionString>().Use("", () =>
                {
                    var connectionString
                        = ConfigurationManager
                        .ConnectionStrings["eventstore"]
                        .ConnectionString;
                    return new MySqlConnectionString(connectionString);
                });

                cfg.For<ReadModelConnectionString>().Use("", () =>
                {
                    var connectionString
                        = ConfigurationManager
                        .ConnectionStrings["dailyops_readmodels"]
                        .ConnectionString;
                    return new ReadModelConnectionString(connectionString);
                });

            });

            return container;
        }


        public static void SendCommand<T>(T command) 
            where T : Command
        {
            bus.Send(command);
        }

        
        private static void Persist<TRepository, TDto>(TDto dto)
            where TRepository : ReadModel
            where TDto : class 
        {
            container.GetInstance<TRepository>().Put(dto);
        }

        
        private static void Mutate<TRepository, TDto>(object id, Action<TDto> mutator)
            where TRepository : ReadModel
            where TDto : class 
        {
            var repo = container.GetInstance<TRepository>();
            TDto dto = repo.FindById<TDto>(id);
            mutator(dto);
            repo.Put(dto);
        }



        public static void CreateReadModelDB()
        {
            ReadModel.CreateReadModelSchema(container.GetInstance<ReadModelConnectionString>());
        }

        public static Plans Plans
        {
            get
            {
                return new Plans(container.GetInstance<ReadModelConnectionString>());
            }
        }
        public static Tasks Tasks
        {
            get
            {
                return new Tasks(container.GetInstance<ReadModelConnectionString>());
            }
        }

        public static CollaborationRepo Collaborators
        {
            get
            {
                return new CollaborationRepo(container.GetInstance<ReadModelConnectionString>());
            }
        }

        public static void GenerateSchemaToFile(string schemaFile)
        {
            string data = ReadModel.ExportReadModelSchema(container.GetInstance<ReadModelConnectionString>());

            System.IO.File.WriteAllText(schemaFile, data.ToString());
        }
    }
}
