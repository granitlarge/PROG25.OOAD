using PROG25.OOAD.SportsBook.Domain.Entities;
using PROG25.OOAD.SportsBook.Domain.ValueObjects;
using PROG25.OOAD.SportsBook.Domain.ValueObjects.Odds;

namespace PROG25.OOAD.SportsBook.Domain.Aggregates.Bets;

public class Bet
{
    private readonly List<BetLeg> _legs;

    protected Bet
    (
        CustomerId customerId,
        List<BetLeg> legs,
        Money stake
    )
    {
        if (stake <= Money.Zero(stake.Currency))
        {
            throw new ArgumentException("Bet stake must be greater than zero.");
        }

        if (legs.Count < 1)
        {
            throw new ArgumentException("A bet must have at least one leg.");
        }

        var legsContainDuplicateMarkets = legs.GroupBy(leg => leg.MarketId).Any(g => g.Count() > 1);
        if (legsContainDuplicateMarkets)
        {
            throw new ArgumentException("A bet cannot have multiple legs on the same market.");
        }

        _legs = legs.ToList();

        Id = new BetId();
        CustomerId = customerId;
        Stake = stake;
        PlacedAt = DateTimeOffset.UtcNow;
        Status = BetStatus.Placed;
    }

    public BetId Id { get; }
    public CustomerId CustomerId { get; }
    public Money Stake { get; }
    public DateTimeOffset PlacedAt { get; }
    public BetStatus Status { get; private set; }
    public Odds Odds => _legs.Where(leg => leg.Status != BetLegStatus.Void).Aggregate(new Odds(1.0m), (acc, leg) => acc * leg.Odds);
    public Money PotentialPayout => Stake * Odds.Value;
    public IReadOnlyList<BetLeg> Legs => _legs.AsReadOnly();

    public void SettleBetLeg(BetLegId betLegId, OutcomeId? winningOutcomeId)
    {
        if (Status != BetStatus.Placed)
        {
            throw new InvalidOperationException("Only placed bets can be settled.");
        }

        if (_legs.All(bl => bl.Id != betLegId))
        {
            throw new ArgumentException("Bet leg ID does not belong to this bet.", nameof(betLegId));
        }

        var legToSettle = _legs.Single(bl => bl.Id == betLegId);
        legToSettle.Settle(winningOutcomeId);

        var allLegsSettledOrVoided = _legs.All(leg => leg.Status != BetLegStatus.Pending);
        if (allLegsSettledOrVoided)
        {
            Status = _legs.All(leg => leg.Status == BetLegStatus.Void || leg.Status == BetLegStatus.Won) ? BetStatus.Won : BetStatus.Lost;
        }
    }

    public void Void()
    {
        if (Status != BetStatus.Placed)
        {
            throw new InvalidOperationException("Only placed bets can be voided.");
        }
        Status = BetStatus.Voided;
    }

    internal static Bet Create(CustomerId customerId, Money stake, List<BetLeg> legs)
    {
        return new Bet(customerId, legs, stake);
    }
}