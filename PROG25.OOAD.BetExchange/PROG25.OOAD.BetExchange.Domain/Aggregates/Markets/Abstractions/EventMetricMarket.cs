using System.Collections.Immutable;
using PROG25.OOAD.BetExchange.Domain.Aggregates.Bets;
using PROG25.OOAD.BetExchange.Domain.Entities.Outcomes;
using PROG25.OOAD.BetExchange.Domain.ValueObjects;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.Events;
using PROG25.OOAD.BetExchange.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.BetExchange.Domain.Aggregates.Markets.Abstractions;

public abstract class EventMetricMarket
{
    protected EventMetricMarket
    (
        EventId eventId,
        EventData eventData,
        EventMetricMarketConfiguration configuration,
        YesNoOutcome yesOutcome,
        YesNoOutcome noOutcome
    )
    {

        if (!yesOutcome.IsYes)
        {
            throw new ArgumentException("The 'yes' outcome must have IsYes set to true.", nameof(yesOutcome));
        }

        if (!noOutcome.IsYes)
        {
            throw new ArgumentException("The 'no' outcome must have IsYes set to false.", nameof(noOutcome));
        }

        if (configuration.Timestamp.HasOccurred(eventData))
        {
            throw new InvalidOperationException("Cannot create an event metric market for an event that has already passed the market's timestamp");
        }

        if (!eventData.Metrics.IsSupportedMetric(configuration.Metric))
        {
            throw new ArgumentException("The event does not support the metric required by the market configuration.", nameof(configuration));
        }

        Id = new MarketId();
        EventId = eventId;
        Configuration = configuration;
        YesOutcome = yesOutcome;
        NoOutcome = noOutcome;
        Status = MarketStatus.Open;
    }

    public MarketId Id { get; }
    public EventId EventId { get; }
    public virtual EventMetricMarketConfiguration Configuration { get; }
    public YesNoOutcome YesOutcome { get; }
    public YesNoOutcome NoOutcome { get; }
    public OutcomeId? WinningOutcomeId { get; private set; }
    public MarketStatus Status { get; private set; }

    public virtual SettlementAttemptStatus TrySettle(EventData eventData)
    {
        if (!Configuration.Timestamp.HasOccurred(eventData))
        {
            if (eventData.Status == EventStatus.Finished)
            {
                Void(); // if the event has finished but the market's timestamp has not occurred, we void the market as it cannot be settled based on the final event data
                return SettlementAttemptStatus.Completed;
            }
            return SettlementAttemptStatus.NotPossible;
        }

        return SettlementAttemptStatus.Possible;
    }

    internal Bet PlaceBet
    (
        CustomerId customerId,
        Money stake,
        Outcome outcome
    )
    {
        if (Status != MarketStatus.Open)
        {
            throw new InvalidOperationException("Bets can only be placed on open markets.");
        }

        if (YesOutcome.Id != outcome.Id && NoOutcome.Id != outcome.Id)
        {
            throw new ArgumentException("Invalid outcome ID for this market.", nameof(outcome));
        }

        return Bet.Create(customerId, stake, Id, outcome.Id);
    }

    public void Settle(OutcomeId winningOutcomeId)
    {
        if (!IsSettleable())
        {
            throw new InvalidOperationException("Only open or closed markets can be settled.");
        }

        if (YesOutcome.Id != winningOutcomeId && NoOutcome.Id != winningOutcomeId)
        {
            throw new ArgumentException("Winning outcome ID must be one of the market's outcomes.", nameof(winningOutcomeId));
        }

        WinningOutcomeId = winningOutcomeId;
        Status = MarketStatus.Settled;
    }

    public void Close()
    {
        if (!IsCloseable())
        {
            throw new InvalidOperationException("Only open markets can be closed.");
        }

        Status = MarketStatus.Closed;
    }

    public void Void()
    {
        if (!IsVoidable())
        {
            throw new InvalidOperationException("Market is already settled or voided.");
        }

        Status = MarketStatus.Voided;
    }

    public bool IsSettleable()
    {
        return Status == MarketStatus.Open || Status == MarketStatus.Closed;
    }

    public bool IsCloseable()
    {
        return Status == MarketStatus.Open;
    }

    public bool IsVoidable()
    {
        return Status != MarketStatus.Voided && Status != MarketStatus.Settled;
    }
}

public enum MarketStatus
{
    Closed, // No bets allowed
    Open, // Bets allowed
    Settled,
    Voided, // Market voided
}

public enum SettlementAttemptStatus
{
    Completed,
    Possible,
    NotPossible,
}