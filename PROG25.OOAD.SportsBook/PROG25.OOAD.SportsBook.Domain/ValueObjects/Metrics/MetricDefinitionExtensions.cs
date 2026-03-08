using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public static class MetricDefinitionExtensions
{
    public static (Scope Scope, MetricDefinition MetricDefinition)[] GetScopedMetrics(this MetricDefinition[] allMetrics, Scope scope)
    {
        return [.. allMetrics.Where(metric => metric.IsValidScope(scope)).Select(metric => metric.ForScope(scope))];
    }
    public static (Scope Scope, MetricDefinition Metric) ForScope(this MetricDefinition metricDefinition, Scope scope)
    {
        if (!metricDefinition.IsValidScopeType(scope.Type))
        {
            throw new InvalidOperationException($"Metric '{metricDefinition.Name}' cannot be used for {scope.Type} scope.");
        }

        return (scope, metricDefinition);
    }
}