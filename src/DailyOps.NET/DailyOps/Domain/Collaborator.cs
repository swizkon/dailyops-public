namespace DailyOps.Domain
{
    using System;
    using System.Collections.Generic;

    using DailyOps.Events;

    using Nuclear.Domain;

    public class Collaborator : AggregateBase
    {
        private readonly IDictionary<CollaboratorRole, List<string>> collaborators = new Dictionary<CollaboratorRole, List<string>>();
        private readonly IDictionary<Guid, string> tasks = new Dictionary<Guid, string>();

        private string username;

        private string displayName;

        public Collaborator(CollaboratorId id, string username, string displayName)
            : this(id)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                AcceptChange(new CollaboratorCreated(id, username, displayName));
            }
        }

        public Collaborator(Guid id) : base(id)
        {
        }

        private void Apply(CollaboratorCreated e)
        {
            this.username = e.Username;
            this.displayName = e.DisplayName;
        }

        public Summary Summary()
        {
            return new Summary(this.displayName, this.username);
        }
    }
}