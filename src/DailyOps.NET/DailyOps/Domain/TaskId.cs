using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyOps.Domain
{
    public sealed class TaskId :IEquatable<TaskId>
    {
        private readonly Guid id;

        private TaskId(Guid id)
        {
            this.id = id;
        }

        public static explicit operator TaskId(Guid id)
        {
            return new TaskId(id);
        }

        public static implicit operator Guid(TaskId taskId)
        {
            return taskId.id;
        }

        public bool Equals(TaskId other)
        {
            return this.id.Equals(other.id);
        }
    }
}
