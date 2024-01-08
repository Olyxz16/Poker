using Poker.Cards;
using Tests.Helpers;

namespace Tests;

public class HighCardTests
{
    
    /// <summary>
    /// HIGH_CARD_TESTS
    /// </summary>

    [Fact]
    public void High_Cards_Neutral() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(12, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.HIGH_CARD, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }
    [Fact]
    public void High_Cards_With_Ace() {
        var flop = new List<Card>() {
            new(3, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(5, CardColor.HEART),
            new(8, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(4, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.HIGH_CARD, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
    [Fact]
    public void High_Cards_With_Highs_Being_Equal() {
        var flop = new List<Card>() {
            new(3, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(5, CardColor.HEART),
            new(8, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(12, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(12, CardColor.HEART), new(4, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.HIGH_CARD, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
    [Fact]
    public void High_Cards_With_Highs_Being_Equal_And_Ace() {
        var flop = new List<Card>() {
            new(3, CardColor.CLUB),
            new(7, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(5, CardColor.HEART),
            new(8, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(1, CardColor.HEART), new(9, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.HIGH_CARD, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }
}