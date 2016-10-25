namespace DailyOps.Domain
{
    public class Summary
    {
        public string Name { get; }

        public string Description { get; }

        public Summary(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}