using System.Collections.Immutable;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Bets;
using PROG25.OOAD.SportsBook.Domain.Aggregates.Events;
using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.Entities.Outcomes;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.EventStates;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.MarketConfigurations.Abstractions;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Markets.Abstractions;

public abstract class Market
{
    protected Market
    (
        EventId eventId,
        MarketConfiguration configuration,
        ISet<Outcome> outcomes
    )
    {
        if (outcomes.Count < 2)
        {
            throw new ArgumentException("Market must have at least two outcomes.", nameof(outcomes));
        }

        Id = new MarketId();
        EventId = eventId;
        Configuration = configuration;
        Outcomes = outcomes.ToImmutableHashSet();
        Status = MarketStatus.Open;
    }

    public MarketId Id { get; }
    public EventId EventId { get; }
    public virtual MarketConfiguration Configuration { get; }
    public virtual ISet<Outcome> Outcomes { get; }
    public OutcomeId? WinningOutcomeId { get; private set; }
    public MarketStatus Status { get; private set; }

    public virtual MarketStatus Settle(EventStatistics previousEventStats, Event @event)
    {
        var statisticsAreForDifferentEvents = previousEventStats.EventId != @event.Id;
        if (statisticsAreForDifferentEvents)
        {
            throw new ArgumentException("Previous and current event states must be for the same event.");
        }

        var marketIsNotAffected = EventId != @event.Id;
        if (marketIsNotAffected)
        {
            throw new ArgumentException("Current event state does not affect the market of this bet.");
        }

        if (@event.Statistics.Status == EventStatus.Cancelled)
        {
            Void();
            return Status;
        }

        return Status;
    }

    internal Bet PlaceBet(CustomerId customerId, Money stake, Outcome outcome)
    {
        if (Status != MarketStatus.Open)
        {
            throw new InvalidOperationException("Bets can only be placed on open markets.");
        }

        if (Outcomes.All(o => o.Id != outcome.Id))
        {
            throw new ArgumentException("Invalid outcome ID for this market.", nameof(outcome));
        }

        return Bet.Create(customerId, stake, [PlaceBetLeg(outcome)]);
    }

    internal BetLeg PlaceBetLeg(Outcome outcome)
    {
        if (Status != MarketStatus.Open)
        {
            throw new InvalidOperationException("Bets can only be placed on open markets.");
        }

        if (Outcomes.All(o => o.Id != outcome.Id))
        {
            throw new ArgumentException("Invalid outcome ID for this market.", nameof(outcome));
        }

        return new BetLeg(Id, outcome.Id, outcome.Odds);
    }

    public void Settle(OutcomeId? winningOutcomeId)
    {
        if (!IsSettleable())
        {
            throw new InvalidOperationException("Only open or closed markets can be settled.");
        }

        if (winningOutcomeId != null && Outcomes.All(outcome => outcome.Id != winningOutcomeId))
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