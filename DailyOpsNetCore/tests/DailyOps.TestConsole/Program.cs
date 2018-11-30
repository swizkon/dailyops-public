using System;
using System.Collections.Generic;
using System.Linq;
using DailyOps.Domain.Model;
using DailyOps.Infra;
using NHibernate;
using FluentNHibernate;

namespace DailyOps.TestConsole
{
    class Program
    {
        static ISessionFactory sessionFactory = PersistenceConfiguration.CreateSessionFactory();

        static void Main(string[] args)
        {
            var username = FindArg("user", args);
            Console.WriteLine($"Hello {username}!");

            using (var session = sessionFactory.OpenSession())
            {
                SeedData(session);
                // PrintInfo(session);
            }

            WithSession(PrintInfo);

            Console.ReadKey();
        }

        private static void SeedData(ISession session)
        {
            var usersToCreate = new[] { "jonas", "jenny" };
            var plansToCreate = new[] { "Desmonds lista", "Signes lista" };

            using (var transaction = session.BeginTransaction())
            {
                var users = usersToCreate.Select(userToCreate => new User { DisplayName = userToCreate });

                foreach (var planToCreate in plansToCreate)
                {
                    var p = new Plan(planToCreate);
                    foreach (var usr in users)
                    {
                        p.Users.Add(usr);
                    }

                    p.Assignments.Add(new Assignment("Plocka undan", Reccurence.Daily));
                    p.Assignments.Add(new Assignment("Städa rummet", Reccurence.Weekly));
                    p.Assignments.Add(new Assignment("Byt lakan", Reccurence.Monthly));
                    p.Assignments.Add(new Assignment("Byt tandborste", Reccurence.Quarterly));
                    p.Assignments.Add(new Assignment("Byt tandborste", Reccurence.Annual));

                    foreach (var a in p.Assignments)
                    {
                        a.MarkAsCompleted(DateTime.Now);
                    }

                    session.SaveOrUpdate(p);
                }

                transaction.Commit();
            }
        }

        private static void PrintInfo(ISession session)
        {
            // retreive all stores and display them
            using (session.BeginTransaction())
            {
                var plans = session.CreateCriteria(typeof(Plan)).List<Plan>();

                foreach (var plan in plans)
                {
                    System.Console.WriteLine("".PadLeft(40, '~'));

                    System.Console.WriteLine(plan.DisplayName.ToUpper());

                    System.Console.WriteLine(string.Join(", ", plan.Users.Select(u => u.DisplayName)));

                    foreach (var assignment in plan.Assignments)
                    {
                        System.Console.WriteLine(" -" + assignment.Summary);
                        System.Console.WriteLine("  " + assignment.Interval.ToString());
                        System.Console.WriteLine("  " + assignment.DateInterval.ToString());
                    }
                }
            }
        }


        private static void WithSession(Action<ISession> action)
        {
            using (var session = sessionFactory.OpenSession())
            {
                action(session);
            }
        }

        private static string FindArg(string name, string[] args)
        {
            return args.Where(a => a.StartsWith(name))
                       .Select(x => x.Replace(name + "=", ""))
                       .FirstOrDefault();
        }
    }
}
