using Poker.Cards;
using Tests.TestTypes;

namespace Tests;

public class CombinationTest
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(4, CardColor.SPADE));
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(12, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(12, CardColor.HEART), new(4, CardColor.SPADE));
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(1, CardColor.HEART), new(9, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }


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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(9, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(2, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(2, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(9, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(2, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(10, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(12, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(4, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(5, CardColor.SPADE));
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Null(winner);
    }





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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(10, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(10, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.THREE_OF_A_KIND, c1.GetCombinationType());
        var c2 = new Combination(p1, flop);
        Assert.Equal(CombinationType.THREE_OF_A_KIND, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, null);
    }


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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(7, CardColor.CLUB), new(9, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(13, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(12, CardColor.CLUB), new(11, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(5, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.STRAIGHT, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }




    /// <summary>
    /// Flush_TESTS
    /// </summary>

    [Fact]
    public void Flush_Neutral1() {
        var flop = new List<Card>() {
            new(2, CardColor.CLUB),
            new(8, CardColor.CLUB),
            new(6, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(7, CardColor.CLUB), new(9, CardColor.CLUB));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.FLUSH, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

    [Fact]
    public void Flush_Neutral2() {
        var flop = new List<Card>() {
            new(1, CardColor.CLUB),
            new(2, CardColor.HEART),
            new(3, CardColor.CLUB),
            new(4, CardColor.HEART),
            new(13, CardColor.HEART)   
        };
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(13, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(6, CardColor.HEART));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.FLUSH, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }




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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(9, CardColor.CLUB));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(13, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(4, CardColor.HEART));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.FULL_HOUSE, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }



    /// <summary>
    /// Four_of_a_kind_TESTS
    /// </summary>

    [Fact]
    public void Four_of_a_kind_Neutral1() {
        var flop = new List<Card>() {
            new(5, CardColor.HEART),
            new(5, CardColor.CLUB),
            new(9, CardColor.CLUB),
            new(11, CardColor.HEART),
            new(13, CardColor.SPADE)   
        };
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(5, CardColor.CLUB), new(5, CardColor.CLUB));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.FOUR_OF_A_KIND, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

    [Fact]
    public void Four_of_a_kind_Neutral2() {
        var flop = new List<Card>() {
            new(5, CardColor.CLUB),
            new(5, CardColor.HEART),
            new(5, CardColor.CLUB),
            new(4, CardColor.HEART),
            new(13, CardColor.HEART)   
        };
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(13, CardColor.CLUB), new(3, CardColor.SPADE));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(5, CardColor.HEART));
        var c2 = new Combination(p2, flop);
        Assert.Equal(CombinationType.FOUR_OF_A_KIND, c2.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p2);
    }
    


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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(6, CardColor.SPADE));
        var p2 = new TestPlayer(100);
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
        var p1 = new TestPlayer(100);
        p1.DrawHand(new(1, CardColor.CLUB), new(13, CardColor.CLUB));
        var p2 = new TestPlayer(100);
        p2.DrawHand(new(13, CardColor.HEART), new(3, CardColor.SPADE));
        var c1 = new Combination(p1, flop);
        Assert.Equal(CombinationType.ROYAL_FLUSH, c1.GetCombinationType());
        var winner = Combination.Compare(p1, p2, flop);
        Assert.Same(winner, p1);
    }

}
