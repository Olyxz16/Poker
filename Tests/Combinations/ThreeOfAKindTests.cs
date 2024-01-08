using Poker.Cards;
using Tests.Helpers;

namespace Tests;

public class ThreeOfAKindTests
{
    /// <summary>
    /// TOAK_TESTS
    /// </summary>

    [Fact]
    public void Three_of_a_kind_Neutral() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(10, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

    [Fact]
    public void Three_of_a_kind_Same() {
        var flop = new List<Card>() {
            new(10, CardColor.CLUB),
            new(10, CardColor.DIAMOND),
            new(2, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new PlayerHelper(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new PlayerHelper(100);
        p2.DrawHand(new(10, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.THREE_OF_A_KIND, c1.GetCombinationType());
        var c2 = new Combination(p1, flop);
        Assert.Equal(CombinationType.THREE_OF_A_KIND, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, null);
    }
}