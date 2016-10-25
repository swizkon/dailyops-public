using DailyOps.Commands;
using DailyOps.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DailyOps.TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Do some stuf..

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("jonas(guest)"), "".Split(','));

            string schemaFile = System.Environment.CurrentDirectory + "\\DailyOps.ReadModels.Schema.mysql";

            Wiring.Proxy.GenerateSchemaToFile(schemaFile);
            Wiring.Proxy.CreateReadModelDB();

            generatePlans();
            // Console.ReadKey();

        }


        static void generatePlans()
        {
            GenerateSignePlan();
            GenerateDesmondPlan();

            GenerateAcmePlan();
        }

        static void GenerateAcmePlan()
        {
            PlanId planId = new PlanId();
            var newPLan = new CreateCollaborativePlan(planId, "ACME plan", "Description goes here...", "BigBoss(guest)");

            Wiring.Proxy.SendCommand(newPLan);

            foreach (string task in "Write executive summary,Tax planning".Split(','))
            {
                Console.WriteLine(task);
                TaskId taskId = (TaskId) Guid.NewGuid();
                Wiring.Proxy.SendCommand(new CreateTask(planId, taskId, task, Reccurence.Daily));
                Thread.Sleep(200);
            }

        }


        private static void GenerateSignePlan()
        {
            var planId = new PlanId();
            var newPLan = new CreateCollaborativePlan(planId, "Signes behov", "Description goes here...", Thread.CurrentPrincipal.Identity.Name);

            Wiring.Proxy.SendCommand(newPLan);

            Wiring.Proxy.SendCommand(new AddCollaborator(planId, "jonas(guest)", "Admin"));
            Wiring.Proxy.SendCommand(new AddCollaborator(planId, "jenny(guest)", "Collaborator"));

            foreach (var task in "D-droppar,Kåvepenin - morgon,Kåvepenin - eftermiddag,Kåvepenin - kväll,Borsta tänderna - morgon,Borsta tänderna - kväll,Kolla naglar".Split(','))
            {
                Console.WriteLine(task);
                var taskId = (TaskId) Guid.NewGuid();
                Wiring.Proxy.SendCommand(new CreateTask(planId, taskId, task, Reccurence.Daily));
                Thread.Sleep(200);
            }

        }

        static void GenerateDesmondPlan()
        {
            var days = CultureInfo.CurrentCulture.DateTimeFormat.DayNames; 
            
            PlanId planId = new PlanId();
            var newPLan = new CreateCollaborativePlan(planId, "Desmonds åtaganden", "Saker som Desmond ska göra för att få veckopeng", Thread.CurrentPrincipal.Identity.Name);

            Wiring.Proxy.SendCommand(newPLan);

            Wiring.Proxy.SendCommand(new AddCollaborator(planId, "jonas(guest)", "Admin"));
            Wiring.Proxy.SendCommand(new AddCollaborator(planId, "jenny(guest)", "Collaborator"));
            Wiring.Proxy.SendCommand(new AddCollaborator(planId, "desmond(guest)", "Auditor"));

            foreach (var taskTitle in "Städa rummet (Varje vecka),Gå igenom läsläxa(Varje vecka på Onsdag),Packa gympakläder(Varje vecka på Torsdag),Kolla naglar(Varje vecka på Söndag)".Split(','))
            {
                Console.WriteLine(taskTitle);

                var reccurence = (taskTitle.Contains("Varje vecka"))
                                ? Reccurence.Weekly 
                                : Reccurence.Daily;
                

                var taskId = (TaskId) Guid.NewGuid();
                var taskCommand = new CreateTask(planId, taskId, taskTitle, reccurence);
                Wiring.Proxy.SendCommand(taskCommand);
                Thread.Sleep(200); // Power nap to allow cool down...

                var complete = new MarkTaskCompleted(taskId, "jonas(guest)", DateTimeOffset.Now);
                Wiring.Proxy.SendCommand(complete);
                Thread.Sleep(200); // Power nap to allow cool down...

                // Check for day names in the culture, 



                // Check if we can generate and reccurence policy, ie "Every week on Tuesdays" => "Every [interval] on [dayName]"

            }

        }
    }
}
