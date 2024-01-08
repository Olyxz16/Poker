using Poker.Cards;
using Tests.Helpers;

namespace Tests;

public class StraightTests
{
    /// <summary>
    /// STRAIGHT_TESTS
    /// </summary>

    [Fact]
    public void Straight_Neutral1() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(8, CardColor.DIAMOND),
            new(6, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(7, CardColor.CLUB), new(9, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.STRAIGHT, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

    [Fact]
    public void Straight_Neutral2() {
        var flop = new List<Card>() {
            new(1, CardColor.CLUB),
            new(2, CardColor.DIAMOND),
            new(3, CardColor.CLUB),
            new(4, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(13, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(5, CardColor.SPADE));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.STRAIGHT, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }


    [Fact]
    public void Straight_Neutral3() {
        var flop = new List<Card>() {
            new(1, CardColor.CLUB),
            new(2, CardColor.DIAMOND),
            new(3, CardColor.CLUB),
            new(10, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(12, CardColor.CLUB), new(11, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(5, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.STRAIGHT, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
}