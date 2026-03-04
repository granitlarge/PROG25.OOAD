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

    public MetricValue Extract(ScopedMetricDefinition scopedMetricDefinition)
    {
        if (!IsSupportedMetric(scopedMetricDefinition.Metric))
        {
            throw new ArgumentException($"The metric '{scopedMetricDefinition.Metric.Name}' is not supported for this event.");
        }

        var metricValue = _metricValues.Single(mv => mv.Definition == scopedMetricDefinition);
        return metricValue;
    }

    public List<MetricValue> ExtractAll(ScopeType scope, MetricDefinition metricDefinition)
    {
        var metricValues = _metricValues.Where(mv => mv.Definition.Scope.Type == scope && mv.Definition.Metric == metricDefinition).ToList();
        return metricValues;
    }

    public bool IsSupportedMetric(MetricDefinition metric)
    {
        return _metricValues.Any(mv => mv.Definition.Metric == metric);
    }
}