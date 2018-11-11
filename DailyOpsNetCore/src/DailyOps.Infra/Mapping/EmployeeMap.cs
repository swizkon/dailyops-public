namespace DailyOps.Infra.Mapping
{
    using NHibernate;
    using FluentNHibernate.Mapping;

    using DailyOps.Domain.Model;

    public class PreferencesMap : ClassMap<Preferences>
    {
        public PreferencesMap()
        {
            Id(x => x.Id);

            Component(x => x.MorningInterval)
                  .ColumnPrefix("Morning");

            Component(x => x.ForenoonInterval)
                  .ColumnPrefix("Forenoon");

            Component(x => x.AfternoonInterval)
                  .ColumnPrefix("Afternoon");

            Component(x => x.EveningInterval)
                  .ColumnPrefix("Evening");
        }
    }

    public class TimeIntervalMap : ComponentMap<TimeInterval>
    {
        public TimeIntervalMap()
        {
            Map(x => x.Starts);
            Map(x => x.Ends);
        }
    }
}