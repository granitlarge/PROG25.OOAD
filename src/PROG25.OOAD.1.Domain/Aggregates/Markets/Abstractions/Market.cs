using PROG25.OOAD.Domain.Entities.MatchEvents;
using PROG25.OOAD.Domain.Entities.Outcome;
using PROG25.OOAD.Domain.ValueObjects;

namespace PROG25.OOAD.Domain.Aggregates.Markets.Abstractions;

public abstract class Market
{
    private readonly List<Outcome> _outcomes = [];

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Market() { } /// EF Core
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    protected Market
    (
        MarketId id,
        MatchId matchId,
        MarketType type,
        List<MatchEventType> settlingEvents,
        List<Outcome> outcomes,
        OutcomeId? settledOutcomeId
    )
    {
        if (outcomes.Count < 2)
        {
            throw new ArgumentException("A market must have at least two outcomes.", nameof(outcomes));
        }

        _outcomes = outcomes.ToList();

        Id = id;
        MatchId = matchId;
        Type = type;
        Status = MarketStatus.Closed;
        SettledOutcomeId = settledOutcomeId;
        SettlingEvents = settlingEvents;
    }

    public MarketId Id { get; }
    public MatchId MatchId { get; }
    public MarketType Type { get; }
    public MarketStatus Status { get; private set; }
    public IReadOnlyList<Outcome> Outcomes => _outcomes;
    public IReadOnlyList<MatchEventType> SettlingEvents { get; }
    public OutcomeId? SettledOutcomeId { get; private set; }

    public void UpdateOdds(OutcomeId outcomeId, Odds newOdds)
    {
        var outcome = Outcomes.Single(o => o.Id == outcomeId);
        outcome.ChangeOdds(newOdds);
    }

    public void Settle(MatchEvent matchEvent, MatchState matchStatistics)
    {
        if (!SettlingEvents.Contains(matchEvent.Type))
            throw new InvalidOperationException("This market cannot be settled with the provided match event.");

        var winningOutcomeId = SettleInternal(matchEvent, matchStatistics);
        if (winningOutcomeId == null)
            return;
        if (!IsSettleable(winningOutcomeId))
            throw new InvalidOperationException("Market cannot be settled with the provided winning outcome.");

        SettledOutcomeId = winningOutcomeId;
        Status = MarketStatus.Settled;
    }

    protected abstract OutcomeId? SettleInternal(MatchEvent matchEvent, MatchState matchStatistics);

    private bool IsSettleable(OutcomeId winningOutcomeId)
    {
        return Status == MarketStatus.Closed && Outcomes.Any(o => o.Id == winningOutcomeId);
    }

    public void Open()
    {
        if (!IsOpenable())
            throw new InvalidOperationException("Market cannot be opened in its current state.");

        Status = MarketStatus.Open;
    }

    public void Close()
    {
        if (!IsCloseable())
            throw new InvalidOperationException("Market cannot be closed in its current state.");

        Status = MarketStatus.Closed;
    }

    public void Cancel()
    {
        if (!IsCancellable())
            throw new InvalidOperationException("Market cannot be cancelled in its current state.");

        Status = MarketStatus.Cancelled;
    }

    private bool IsCancellable()
    {
        return Status != MarketStatus.Settled;
    }

    private bool IsCloseable()
    {
        return Status == MarketStatus.Open;
    }

    private bool IsOpenable()
    {
        return Status == MarketStatus.Closed;
    }

    internal void EnsureBetCanBePlaced(OutcomeId id)
    {
        if (Outcomes.All(o => o.Id != id))
            throw new InvalidOperationException("Outcome does not exist in this market.");
    }
}