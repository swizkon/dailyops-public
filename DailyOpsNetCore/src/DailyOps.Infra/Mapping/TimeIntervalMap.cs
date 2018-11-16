namespace DailyOps.Infra.Mapping
{
    using NHibernate;
    using FluentNHibernate.Mapping;

    using DailyOps.Domain.Model;

    public class TimeIntervalMap : ComponentMap<TimeInterval>
    {
        public TimeIntervalMap()
        {
            Map(x => x.Starts);
            Map(x => x.Ends);
        }
    }
}