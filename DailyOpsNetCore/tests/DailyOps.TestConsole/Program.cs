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
                    var usersToCreate = new[] { "jonas", "jenny" };

                    foreach (var userToCreate in usersToCreate)
                    {
                        var u = new User { DisplayName = userToCreate };
                        
                        if(userToCreate == "jonas")
                        u.PersonalPreferences.MorningInterval.Starts = TimeSpan.FromHours(05);
                        
                        session.SaveOrUpdate(u);
                    }

                    transaction.Commit();
                }

                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    var users = session.CreateCriteria(typeof(User)).List<User>();

                    foreach (var user in users)
                    {
                        System.Console.WriteLine(user.Id);
                        System.Console.WriteLine(user.DisplayName);
                        System.Console.WriteLine(user.PersonalPreferences.MorningInterval);
                        System.Console.WriteLine(user.PersonalPreferences.ForenoonInterval);
                        System.Console.WriteLine(user.PersonalPreferences.AfternoonInterval);
                        System.Console.WriteLine(user.PersonalPreferences.EveningInterval);
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
