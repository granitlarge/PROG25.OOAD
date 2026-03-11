using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Periods;

namespace PROG25.OOAD.BetExchange.Domain.Entities;

public class EventType
{
    private readonly ImmutableHashSet<MetricDefinition> _supportedMetrics;
    private readonly PeriodRules _periodRules;

    public EventType
    (
        EventTypeEnum eventType,
        HashSet<MetricDefinition> supportedMetrics,
        PeriodRules periodRules
    )
    {
        _supportedMetrics = [.. supportedMetrics];
        _periodRules = periodRules;

        Id = new EventTypeId();
        Type = eventType;
    }

    public EventTypeId Id { get; }

    public EventTypeEnum Type { get; }

    public ImmutableHashSet<MetricDefinition> SupportedMetrics => _supportedMetrics;

    public Period? GetPeriod(EventData eventData)
    {
        return _periodRules.GetPeriod(eventData);
    }
}