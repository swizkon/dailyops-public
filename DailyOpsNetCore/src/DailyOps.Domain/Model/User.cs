namespace DailyOps.Domain.Model
{
    public class User
    {
        public virtual long Id { get; protected set; }

        public virtual string DisplayName { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual Preferences PersonalPreferences { get; set; }

        public User()
        {
            PersonalPreferences = new Preferences();
        }
    }
}
