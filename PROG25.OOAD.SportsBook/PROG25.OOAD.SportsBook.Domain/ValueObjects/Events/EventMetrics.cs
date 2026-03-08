using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Values;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Events;

public record EventMetrics
{
    private readonly ISet<MetricValue> _metricValues;

    public EventMetrics(ISet<MetricValue> metricValues)
    {
        _metricValues = metricValues;
    }

    public MetricValue Extract(Scope scope, MetricDefinition metricDefinition)
    {
        return ExtractAll(scope.Type, metricDefinition).Single(mv => mv.Scope == scope);
    }

    public ImmutableHashSet<MetricValue> ExtractAll(ScopeType scopeType, MetricDefinition metricDefinition)
    {
        if (!IsSupportedMetric(metricDefinition))
        {
            throw new InvalidOperationException("Unsupported metric");
        }
        return metricDefinition.Aggregate(scopeType, _metricValues.Where(mv => mv.Metric == metricDefinition).ToImmutableHashSet());
    }

    public bool IsSupportedMetric(MetricDefinition metric)
    {
        return _metricValues.Any(mv => mv.Metric == metric);
    }
}