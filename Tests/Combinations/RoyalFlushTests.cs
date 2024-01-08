using Poker.Cards;
using Tests.Helpers;

namespace Tests;

public class RoyalFlushTests
{
    /// <summary>
    /// ROYAL_FLUSH_TESTS
    /// </summary>

    [Fact]
    public void Royal_Flush_Test1() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(11, CardColor.CLUB),
            new(12, CardColor.CLUB),
            new(13, CardColor.CLUB),
            new(4, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.ROYAL_FLUSH, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
    [Fact]
    public void Royal_Flush_Test2() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(11, CardColor.CLUB),
            new(12, CardColor.CLUB),
            new(3, CardColor.CLUB),
            new(4, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(13, CardColor.CLUB));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.ROYAL_FLUSH, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

}