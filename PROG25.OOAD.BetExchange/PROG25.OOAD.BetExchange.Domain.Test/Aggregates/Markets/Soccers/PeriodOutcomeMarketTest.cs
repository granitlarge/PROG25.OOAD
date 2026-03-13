using System.Collections.Immutable;
using System.Text.Json;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Events;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Soccers;
using PROG25.OOAD.BetExchange.Domain.Entities;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Sports;

namespace PROG25.OOAD.BetExchange.Domain.Test.Aggregates.Markets.Soccers;

public class PeriodOutcomeMarketTest
{
    [Test]
    public void GenerateSoccerMarkets()
    {
        var teams = new HashSet<Team>
        {
            new("Juventus", [new Player(new Domain.ValueObjects.PlayerId(), "Zlatan Ibrahimovic")]),
            new("Inter", [new Player(new Domain.ValueObjects.PlayerId(), "Henrik Larsson")])
        };

        var eventMetrics = new EventMetrics
        (
            new HashSet<MetricValue>
            {
                new(new DimensionValue(ImmutableDictionary<string, object>.Empty, Soccer.EmptyDimension), Soccer.Period, 1),
            }
        );
        var eventData = new EventData
        (
                eventMetrics,
                Domain.ValueObjects.EventStatus.Scheduled,
                DateTime.UtcNow.AddDays(1)
        );
        var e = new Event(new EventType(Domain.ValueObjects.EventTypeEnum.Soccer, [.. Soccer.AllMetrics], new SoccerPeriodRules()), teams, eventData);
        var markets = PeriodOutcomeMarket.Create(e);
        Console.WriteLine(JsonSerializer.Serialize(markets.Select(m => m.Configuration.Name), new JsonSerializerOptions { WriteIndented = true }));
    }
}