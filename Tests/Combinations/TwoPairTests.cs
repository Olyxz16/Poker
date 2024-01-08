using Poker.Cards;
using Tests.Helpers;

namespace Tests;

public class TwoPairTests
{
    /// <summary>
    /// TWO_PAIR_TESTS
    /// </summary>
    
    [Fact]
    public void Two_Pair_Neutral_P1() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(6, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
    [Fact]
    public void Two_Pair_Neutral_P2() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(9, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }

    [Fact]
    public void Two_Pair_Equal_Compare_High_Card() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(10, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(2, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }

    [Fact]
    public void Two_Pair_Compare_Different() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(6, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(13, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }


    [Fact]
    public void Two_Pair_On_Flop_Best_Hand_Pair() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(10, CardColor.DIAMOND),
            new(6, CardColor.CLUB),
            new(6, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(12, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(4, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

    [Fact]
    public void Two_Pair_On_Flop_Worst_Hand_Pair() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(10, CardColor.DIAMOND),
            new(6, CardColor.CLUB),
            new(6, CardColor.HEART),
            new(4, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(4, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(5, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Null(winner);
    }
}