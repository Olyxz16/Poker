using Poker.Cards;
using Tests.Helpers;

namespace Tests;


public class PairTests 
{
    /// <summary>
    /// PAIR_TESTS
    /// </summary>
    
    [Fact]
    public void Pair_Neutral_P1() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.PAIR, c1.GetCombinationType());
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.HIGH_CARD, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
    [Fact]
    public void Pair_Neutral_P2() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(9, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.PAIR, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }

    [Fact]
    public void Pair_Equal_Compare_High_Card() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(2, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.PAIR, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }

    [Fact]
    public void Pair_Compare_Different() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.PAIR, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
}