using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Dimensions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Definitions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Expressions;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Metrics.Values;

namespace PROG25.OOAD.BetExchange.Domain.Test.ValueObjects.Metrics;

public class MetricExpressionTest
{
    [Test]
    public void ConstantMetricExpression_Evaluate_ReturnsValue()
    {
        var testMetrics = new EventMetrics(new HashSet<MetricValue>());
        var expression = new ConstantMetricExpression<decimal>(42m);
        var result = expression.Evaluate(testMetrics);
        Assert.That(result, Is.EqualTo(42m));
    }

    [Test]
    public void ArithmeticMetricExpression_Evaluate_ReturnsCorrectResult()
    {
        var testMetrics = new EventMetrics(new HashSet<MetricValue>());
        var left = new ConstantMetricExpression<decimal>(10m);
        var right = new ConstantMetricExpression<decimal>(5m);
        foreach (var operation in new[] { ArithmeticOperation.Addition, ArithmeticOperation.Subtraction, ArithmeticOperation.Multiplication, ArithmeticOperation.Division })
        {
            var expression = new ArithmeticMetricExpression(operation, left, right);
            var result = expression.Evaluate(testMetrics);
            var expected = operation switch
            {
                ArithmeticOperation.Addition => 15m,
                ArithmeticOperation.Subtraction => 5m,
                ArithmeticOperation.Multiplication => 50m,
                ArithmeticOperation.Division => 2m,
                _ => throw new NotImplementedException()
            };
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [Test]
    public void ComparisonMetricExpression_Evaluate_ReturnsCorrectResult()
    {
        var testMetrics = new EventMetrics(new HashSet<MetricValue>());

        var testCases = new[]
        {
            (Left: 10m, Right: 5m, Expected: ComparisonResult.GreaterThan),
            (Left: 5m, Right: 10m, Expected: ComparisonResult.LessThan),
            (Left: 7m, Right: 7m, Expected: ComparisonResult.Equal)
        };

        foreach (var (Left, Right, Expected) in testCases)
        {
            var left = new ConstantMetricExpression<decimal>(Left);
            var right = new ConstantMetricExpression<decimal>(Right);
            var expression = new ComparisonMetricExpression(left, right, FaultTolerance.Zero);
            var result = expression.Evaluate(testMetrics);
            Assert.That(result, Is.EqualTo(Expected));
        }
    }

    [Test]
    public void FilteredAndAggregatedMetricExpression_Evaluate_ReturnsCorrectResult()
    {
        var dimensionDefinition = new DimensionDefinition(new Dictionary<string, Type>
        {
            ["Dimension1"] = typeof(string),
            ["Dimension2"] = typeof(string)
        }.ToImmutableDictionary());

        var metricDefinition = new MetricDefinition(0, decimal.MaxValue, FaultTolerance.Zero, "TestMetric", dimensionDefinition, Aggregation.Sum);
        var filters = ImmutableHashSet.Create
        (
            new DimensionFilter
            (
                new Dictionary<string, object> { { "Dimension1", "Dim1Value1" } }.ToImmutableDictionary(),
                dimensionDefinition
            )
        );

        var expression = new FilteredAndAggregatedMetricExpression(metricDefinition, filters);
        var testMetrics = new EventMetrics(new HashSet<MetricValue>
        {
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value1"} }.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 10m),
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value2" }, {"Dimension2", "Dim2Value2"} }.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 20m),
            new(new (new Dictionary<string, object> { { "Dimension1", "Dim1Value1" }, {"Dimension2", "Dim2Value1"} }.ToImmutableDictionary(), dimensionDefinition), metricDefinition, 30m)
        });

        var result = expression.Evaluate(testMetrics);
        Assert.That(result, Is.EqualTo(40m)); // Only the values with Dimension1=Value1 should be aggregated
    }
}