namespace DailyOps.Domain
{
    using System;

    [Flags]
    public enum CollaboratorRole
    {
        Owner = 512,
        Admin = 256,
        Collaborator = 128,
        Auditor = 64
    }
}