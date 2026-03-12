using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;

namespace PROG25.OOAD.BetExchange.Domain.Test.ValueObjects.Metrics;

public class MetricValueTest
{
    [Test]
    public void MetricValue_Equality_Success()
    {
        var dimensionDef = new DimensionDefinition
        (
            new Dictionary<string, Type>
            {
                { "Dim1", typeof(string) }
            }.ToImmutableDictionary()
        );
        var metricDef = new MetricDefinition(0, 10, FaultTolerance.Zero, "TestMetricDefinition1", dimensionDef, Aggregation.Sum);

        var mv1 = new MetricValue
        (
            new DimensionValue
            (
                new Dictionary<string, object>
                {
                    {"Dim1", "Hello!"}
                }.ToImmutableDictionary(),
                dimensionDef
            ),
            metricDef,
            1.0m
        );
        var mv2 = new MetricValue
        (
            new DimensionValue
            (
                new Dictionary<string, object>
                {
                    {"Dim1", "Hello!"}
                }.ToImmutableDictionary(),
                dimensionDef
            ),
            metricDef,
            1.0m
        );
        Assert.That(mv1, Is.EqualTo(mv2));
        Assert.That(mv1 == mv2, Is.True);
    }
}