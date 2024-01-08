using Poker.Cards;
using Tests.Helpers;

namespace Tests;

public class StraightFlashTests
{
    /// <summary>
    /// Straight_flush_TESTS
    /// </summary>

    [Fact]
    public void Straight_flush_Neutral1() {
        var flop = new List<Card>() {
            new(2, CardColor.CLUB),
            new(5, CardColor.CLUB),
            new(9, CardColor.HEART),
            new(6, CardColor.CLUB),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(4, CardColor.CLUB), new(3, CardColor.CLUB));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.STRAIGHT_FLUSH, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

    [Fact]
    public void Straight_flush_Neutral2() {
        var flop = new List<Card>() {
            new(8, CardColor.CLUB),
            new(2, CardColor.HEART),
            new(5, CardColor.CLUB),
            new(4, CardColor.HEART),
            new(3, CardColor.HEART)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(13, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(1, CardColor.HEART), new(5, CardColor.HEART));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.STRAIGHT_FLUSH, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }

    [Fact]
    public void Flush_And_Straight_But_Not_Straight_Flush() {
        var flop = new List<Card>() {
            new(8, CardColor.CLUB),
            new(2, CardColor.CLUB),
            new(5, CardColor.CLUB),
            new(1, CardColor.HEART),
            new(3, CardColor.CLUB)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(4, CardColor.SPADE), new(3, CardColor.CLUB));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.FLUSH, c1.GetCombinationType());
    }
}