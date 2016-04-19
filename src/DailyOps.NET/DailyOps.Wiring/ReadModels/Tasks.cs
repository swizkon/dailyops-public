using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Wiring.ReadModels
{
    public class Tasks : ReadModel
    {
        public Tasks(ReadModelConnectionString connectionString)
            : base(connectionString)
        {

        }


        public IEnumerable<TaskDto> TaskInPlan(Guid planId)
        {
            return base.Query<TaskDto>((session) =>
            {
                return session.QueryOver<TaskDto>().Where(t => t.PlanId == planId).List();
            });
        }

        public void Add(TaskDto task)
        {
            /* Create a session and execute a query: */
            base.Transaction((session) =>
            {
                session.SaveOrUpdate(task);
            });
        }
    }


    internal class TaskMap : ClassMapping<TaskDto>
    {
        public TaskMap()
        {
            this.Table("dailyops_tasks");
            this.Id(p => p.TaskId);
            this.Property(p => p.PlanId);
            this.Property(p => p.Title);
            this.Property(p => p.Description);
        }
    }

    public class TaskDto
    {
        public virtual Guid PlanId { get; set; }

        public virtual Guid TaskId { get; set; }

        public virtual string Title { get; set; }

        public virtual string Description { get; set; }
    }


    internal class CompletedTaskMap : ClassMapping<CompletedTaskDto>
    {
        public CompletedTaskMap()
        {
            this.Table("dailyops_completed_tasks");
            this.Id(p => p.TaskId);
            this.Property(p => p.PlanId);
            this.Property(p => p.Title);
            this.Property(p => p.LastCompleted);
            this.Property(p => p.CompletedBy);
        }
    }

    public class CompletedTaskDto
    {
        public virtual Guid PlanId { get; set; }

        public virtual Guid TaskId { get; set; }

        public virtual string Title { get; set; }

        public virtual string LastCompleted { get; set; }

        public virtual string CompletedBy { get; set; }
    }



    internal class RemainingTaskMap : ClassMapping<RemainingTaskDto>
    {
        public RemainingTaskMap()
        {
            this.Table("dailyops_remaining_tasks");
            this.Id(p => p.TaskId);
            this.Property(p => p.Title);
            this.Property(p => p.PlanName);
            this.Property(p => p.TaskType);
        }
    }

    public class RemainingTaskDto
    {
        public virtual Guid TaskId { get; set; }

        public virtual string Title { get; set; }

        public virtual string PlanName { get; set; }

        public virtual string TaskType { get; set; }
    }

}
