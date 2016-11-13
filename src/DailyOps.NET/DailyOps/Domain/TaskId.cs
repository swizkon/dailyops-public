using System;

namespace DailyOps.Domain
{
    public sealed class TaskId : IEquatable<TaskId>
    {
        private readonly Guid id;

        private TaskId(Guid id)
        {
            this.id = id;
        }

        public static TaskId Create()
        {
            return new TaskId(Guid.NewGuid());
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
            return this.id.Equals(other?.id);
        }
    }
}
