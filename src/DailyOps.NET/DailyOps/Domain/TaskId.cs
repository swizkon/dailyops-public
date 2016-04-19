using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Domain
{
    public sealed class TaskId
    {
        private Guid id;

        public TaskId()
            : this(Guid.NewGuid())
        {
        }

        public TaskId(Guid id)
        {
            this.id = id;
        }

        public static explicit operator TaskId(Guid id)
        {
            return new TaskId(id);
        }

        public static explicit operator Guid(TaskId taskId)
        {
            return taskId.id;
        }

    }
}
