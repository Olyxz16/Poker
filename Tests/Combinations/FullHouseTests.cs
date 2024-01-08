using Poker.Cards;
using Tests.Helpers;

namespace Tests;

public class FullHouseTests
{
    /// <summary>
    /// Full_House_TESTS
    /// </summary>

    [Fact]
    public void Full_House_Neutral1() {
        var flop = new List<Card>() {
            new(5, CardColor.HEART),
            new(5, CardColor.CLUB),
            new(9, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(9, CardColor.CLUB));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.FULL_HOUSE, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

    [Fact]
    public void Full_House_Neutral2() {
        var flop = new List<Card>() {
            new(5, CardColor.CLUB),
            new(5, CardColor.HEART),
            new(5, CardColor.CLUB),
            new(4, CardColor.HEART),
            new(13, CardColor.HEART)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(13, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(4, CardColor.HEART));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.FULL_HOUSE, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }
}