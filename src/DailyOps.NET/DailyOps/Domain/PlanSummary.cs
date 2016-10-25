namespace DailyOps.Domain
{
    public class PlanSummary
    {
        public string Name { get; }

        public string Description { get; }

        public PlanType PlanType { get; }

        public PlanSummary(string name, string description, PlanType planType)
        {
            this.Name = name;
            this.Description = description;
            this.PlanType = planType;
        }
    }
}