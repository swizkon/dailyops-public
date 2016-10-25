namespace DailyOps.Domain
{
    using System;

    public sealed class CollaboratorId
    {
        private readonly Guid id;

        public CollaboratorId() : this(Guid.NewGuid())
        {
        }

        public CollaboratorId(Guid id)
        {
            this.id = id;
        }

        public static explicit operator CollaboratorId(Guid id)
        {
            return new CollaboratorId(id);
        }

        public static implicit operator Guid(CollaboratorId collaboratorId)
        {
            return collaboratorId.id;
        }
    }
}