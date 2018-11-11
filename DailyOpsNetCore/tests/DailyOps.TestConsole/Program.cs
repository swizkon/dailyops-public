using System;
using System.Linq;
using DailyOps.Domain.Model;
using DailyOps.Infra;

namespace DailyOps.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var username = FindArg("user", args);
            Console.WriteLine($"Hello {username}!");

            var sessionFactory = PersistenceConfiguration.CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    // create a couple of Stores each with some Products and Employees
                    var u = new User { DisplayName = username };
                    
                    session.SaveOrUpdate(u);

                    transaction.Commit();
                }

                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    var users = session.CreateCriteria(typeof(User)).List<User>();

                    foreach (var user in users)
                    {
                        System.Console.WriteLine(user.DisplayName);
                    }
                }
            }

            Console.ReadKey();
        }

        private static string FindArg(string name, string[] args)
        {
            return args.Where(a => a.StartsWith(name))
                       .Select(x => x.Replace(name + "=", ""))
                       .FirstOrDefault();
        }
    }
}
