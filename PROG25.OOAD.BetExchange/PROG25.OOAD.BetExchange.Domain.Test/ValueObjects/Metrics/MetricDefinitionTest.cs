using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;

namespace PROG25.OOAD.BetExchange.Domain.Test.ValueObjects.Metrics;

public class MetricDefinitionTest
{
    [Test]
    public void MetricDefinition_Creation_SucceedsWithValidParameters()
    {
        var dimension = new DimensionDefinition(new Dictionary<string, Type> { { "TestDimension", typeof(string) } }.ToImmutableDictionary());
        var metricDefinition = new MetricDefinition(0m, 100m, FaultTolerance.Zero, "TestMetric", dimension, Aggregation.Sum);

        Assert.Multiple(() =>
        {
            Assert.That(metricDefinition.Name, Is.EqualTo("TestMetric"));
            Assert.That(metricDefinition.Dimension, Is.EqualTo(dimension));
        });
    }

    [Test]
    public void MetricDefinition_Creation_FailsWithInvalidName()
    {
        var dimension = new DimensionDefinition(new Dictionary<string, Type> { { "TestDimension", typeof(string) } }.ToImmutableDictionary());

        Assert.Throws<ArgumentException>(() => new MetricDefinition(0m, 100m, FaultTolerance.Zero, "", dimension, Aggregation.Sum));
        Assert.Throws<ArgumentException>(() => new MetricDefinition(0m, 100m, FaultTolerance.Zero, "   ", dimension, Aggregation.Sum));
        Assert.Throws<ArgumentException>(() => new MetricDefinition(0m, 100m, FaultTolerance.Zero, null!, dimension, Aggregation.Sum));
    }

    [Test]
    public void IsValidMetricValue_ReturnsTrueForValuesWithinBounds()
    {
        var dimension = new DimensionDefinition(new Dictionary<string, Type> { { "TestDimension", typeof(string) } }.ToImmutableDictionary());
        var metricDefinition = new MetricDefinition(0m, 100m, FaultTolerance.Zero, "TestMetric", dimension, Aggregation.Sum);

        Assert.Multiple(() =>
        {
            Assert.That(metricDefinition.IsValidMetricValue(0m), Is.True);
            Assert.That(metricDefinition.IsValidMetricValue(50m), Is.True);
            Assert.That(metricDefinition.IsValidMetricValue(100m), Is.True);
        });
    }

    [Test]
    public void IsValidMetricValue_ReturnsFalseForValuesOutsideBounds()
    {
        var dimension = new DimensionDefinition(new Dictionary<string, Type> { { "TestDimension", typeof(string) } }.ToImmutableDictionary());
        var metricDefinition = new MetricDefinition(0m, 100m, FaultTolerance.Zero, "TestMetric", dimension, Aggregation.Sum);

        Assert.Multiple(() =>
        {
            Assert.That(metricDefinition.IsValidMetricValue(-1m), Is.False);
            Assert.That(metricDefinition.IsValidMetricValue(101m), Is.False);
        });
    }

    [Test]
    public void Aggregate_ReturnsCorrectResultForSum()
    {
        var dimension = new DimensionDefinition(new Dictionary<string, Type> { }.ToImmutableDictionary());
        var metricDefinition = new MetricDefinition(0m, 100m, FaultTolerance.Zero, "TestMetric", dimension, Aggregation.Sum);

        var metricValues = new HashSet<MetricValue>
        {
            new(new DimensionValue(ImmutableDictionary<string, object>.Empty, dimension), metricDefinition, 10m),
            new(new DimensionValue(ImmutableDictionary<string, object>.Empty, dimension), metricDefinition, 20m),
            new(new DimensionValue(ImmutableDictionary<string, object>.Empty, dimension), metricDefinition, 30m)
        }.ToImmutableHashSet();

        var result = metricDefinition.Aggregate(metricValues);
        Assert.That(result, Is.EqualTo(60m));
    }

    [Test]
    public void Aggregate_ThrowsExceptionForInvalidMetricValues()
    {
        var dimension = new DimensionDefinition(new Dictionary<string, Type> { }.ToImmutableDictionary());
        var metricDefinition1 = new MetricDefinition(0m, 100m, FaultTolerance.Zero, "TestMetric1", dimension, Aggregation.Sum);
        var metricDefinition2 = new MetricDefinition(0m, 100m, FaultTolerance.Zero, "TestMetric2", dimension, Aggregation.Sum);

        var metricValues = new HashSet<MetricValue>
        {
            new(new DimensionValue(ImmutableDictionary<string, object>.Empty, dimension), metricDefinition2, 10m),
            new(new DimensionValue(ImmutableDictionary<string, object>.Empty, dimension), metricDefinition2, 20m)
        }.ToImmutableHashSet();

        Assert.Throws<ArgumentException>(() => metricDefinition1.Aggregate(metricValues));
    }

    [Test]
    public void Filter_ReturnsCorrectSubsetOfMetricValues()
    {
        // Test the following:
        // 1) Returns only those metric values whose dimensions agree with the filter.
        // 2) If there are multiple filters for the same dimension, return the metricvalues that agree with ANY of those filters.
        var dimensionDefinition = new DimensionDefinition(new Dictionary<string, Type>
        {
            ["Dimension1"] = typeof(string),
            ["Dimension2"] = typeof(string)
        }.ToImmutableDictionary());

        var metricDefinition = new MetricDefinition(0, decimal.MaxValue, FaultTolerance.Zero, "TestMetric", dimensionDefinition, Aggregation.Sum);
        var testMetricValues = new HashSet<MetricValue>
        {
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value1"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 10m),
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value2" }, {"Dimension2", "Dim2Value2"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 20m),
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value3" }, {"Dimension2", "Dim2Value3"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 30m)
        }.ToImmutableHashSet();

        var testCases = new[]
        {
            (
                Filters: ImmutableHashSet.Create
                (
                    new DimensionFilter
                    (
                        new Dictionary<string, object>{{"Dimension1", "Dim1Value1"}}.ToImmutableDictionary(),
                        dimensionDefinition
                    )
                ),
                Expected: new MetricValue[]
                {
                    new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value1"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 10m),
                }.ToImmutableHashSet()
            ),
            (
                Filters:
                [
                    new DimensionFilter
                    (
                        new Dictionary<string, object>{{"Dimension1", "Dim1Value1"}}.ToImmutableDictionary(),
                        dimensionDefinition
                    ),
                    new DimensionFilter
                    (
                        new Dictionary<string, object>{{"Dimension1", "Dim1Value3"}}.ToImmutableDictionary(),
                        dimensionDefinition
                    ),
                    new DimensionFilter
                    (
                        new Dictionary<string, object>{{"Dimension2", "Dim2Value3"}}.ToImmutableDictionary(),
                        dimensionDefinition
                    )
                ],
                Expected: new MetricValue[]
                {
                    new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value1"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 10m),
                    new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value3" }, {"Dimension2", "Dim2Value3"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 30m)
                }.ToImmutableHashSet()
            )
        };

        foreach (var (Filters, Expected) in testCases)
        {
            var actual = metricDefinition.Filter(Filters, testMetricValues);
            Assert.That(actual, Has.Count.EqualTo(Expected.Count));
            Assert.That(actual.All(a => Expected.Any(e => a == e)) && Expected.All(e => actual.Any(a => e == a)), Is.True);
        }
    }

    [Test]
    public void GroupBy_Returns_CorrectResult()
    {
        var dimensionDefinition = new DimensionDefinition(new Dictionary<string, Type>
        {
            ["Dimension1"] = typeof(string),
            ["Dimension2"] = typeof(string)
        }.ToImmutableDictionary());

        var metricDefinition = new MetricDefinition(0, decimal.MaxValue, FaultTolerance.Zero, "TestMetric", dimensionDefinition, Aggregation.Sum);
        var testMetricValues = new HashSet<MetricValue>
        {
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value1"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 10m),
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value2"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 20m),
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value2" }, {"Dimension2", "Dim2Value3"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 30m)
        }.ToImmutableHashSet();

        var testCases = new[]
        {
            (
                GroupingDimensions: new HashSet<string> { "Dimension1" }.ToImmutableHashSet(),
                Expected: new[]
                {
                    (
                        Key: new DimensionFilter(new Dictionary<string, object> { { "Dimension1", "Dim1Value1" } }.ToImmutableDictionary(), dimensionDefinition),
                        Values: new MetricValue[]
                        {
                                new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value1"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 10m),
                                new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value2"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 20m)
                        }.ToImmutableHashSet()
                    ),
                    (
                        Key: new DimensionFilter(new Dictionary<string, object> { { "Dimension1", "Dim1Value2" } }.ToImmutableDictionary(), dimensionDefinition),
                        Values: new MetricValue[]
                        {
                            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value2" }, {"Dimension2", "Dim2Value3"}}.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 30m)
                        }.ToImmutableHashSet()
                    )
                }.ToImmutableHashSet()
            )
        };

        for (var i = 0; i < testCases.Length; i++)
        {
            var (GroupingDimensions, Expected) = testCases[i];
            var actual = metricDefinition.GroupBy(GroupingDimensions, testMetricValues);

            Assert.Multiple(() =>
            {
                Assert.That(actual.All(kv => Expected.Any(ekv => ekv.Key == kv.Key && ekv.Values.SetEquals(kv.Values))), Is.True);
                Assert.That(Expected.All(ekv => actual.Any(kv => ekv.Key == kv.Key && ekv.Values.SetEquals(kv.Values))), Is.True);
            });
        }
    }
}