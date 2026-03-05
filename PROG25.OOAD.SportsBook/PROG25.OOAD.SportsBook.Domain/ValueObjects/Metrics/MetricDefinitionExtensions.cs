using PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Scopes;

namespace PROG25.OOAD.SportsBook.Domain.ValueObjects.Metrics;

public static class MetricDefinitionExtensions
{
    public static (Scope Scope, MetricDefinition Metric) ForScope(this MetricDefinition metricDefinition, Scope scope)
    {
        if (!metricDefinition.IsValidScopeType(scope.Type))
        {
            throw new InvalidOperationException($"Metric '{metricDefinition.Name}' cannot be used for {scope.Type} scope.");
        }

        return (scope, metricDefinition);
    }
}