using DailyOps.Commands;
using DailyOps.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DailyOps.TestConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Do some stuf..


            string schemaFile = System.Environment.CurrentDirectory + "\\DailyOps.ReadModels.Schema.mysql";

            Wiring.Proxy.GenerateSchemaToFile(schemaFile);
            Wiring.Proxy.CreateReadModelDB();



            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("jonas@jerndin.se"), "".Split(','));

            PlanId planId = new PlanId();


            var newPLan = new CreateCollaborativePlan(planId, "Signes", "Description goes here...", Thread.CurrentPrincipal.Identity.Name);


            Wiring.Proxy.SendCommand(newPLan);

            foreach (string task in "D-droppar,Kåvepenin x 3,Borsta tänderna,Kolla naglar".Split(','))
            {
                TaskId taskId = new TaskId();
                Wiring.Proxy.SendCommand(new CreateTask(planId, taskId, task, Domain.TaskType.Daily));
                Thread.Sleep(500);
            }

            AddCollaborator cmd = new AddCollaborator(planId, "jonas@jerndin.se", "Admin");
            Wiring.Proxy.SendCommand(cmd);

            // Console.ReadKey();

        }
    }
}
