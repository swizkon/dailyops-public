using System;
using System.Linq;

namespace DailyOps.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = FindArg("user", args);
            Console.WriteLine($"Hello {user}!");
            
        }

        private static string FindArg(string name, string[] args)
        {
            return args.Where(a => a.StartsWith(name))
                       .Select(x =>x.Replace(name + "=", ""))
                       .FirstOrDefault();
        }
    }
}
