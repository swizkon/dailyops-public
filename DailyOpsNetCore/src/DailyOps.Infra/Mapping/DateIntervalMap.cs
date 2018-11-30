namespace DailyOps.Infra.Mapping
{
    using FluentNHibernate.Mapping;

    using DailyOps.Domain.Model;

    public class DateWindowMap : ComponentMap<DateWindow>
    {
        public DateWindowMap()
        {
            Map(x => x.Reapperance);
            Map(x => x.ClosingDate);
        }
    }
}