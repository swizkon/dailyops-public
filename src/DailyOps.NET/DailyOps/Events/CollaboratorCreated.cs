namespace DailyOps.Events
{
    using DailyOps.Domain;

    using Nuclear.Domain;

    public class CollaboratorCreated : DomainEvent
    {
        public readonly CollaboratorId Id;
        public readonly string DisplayName;
        public readonly string Username;

        public CollaboratorCreated(CollaboratorId id, string username, string displayName)
        {
            this.Id = id;
            this.Username = username;
            this.DisplayName = displayName;
        }
    }
}