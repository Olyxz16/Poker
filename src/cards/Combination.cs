using Poker.Players;

namespace Poker.Cards;

public struct Combination
{

    private int _flag;


    private Combination(Player player, List<Card> flop) {
        _flag = 0;
        var cards = OrderCards(player, flop);
        int pairs = NumberOfPairs(cards);
        _flag |= IsFlush(cards) ? (int)CombinationType.FLUSH: 0;
        _flag |= IsStraight(cards) ? (int)CombinationType.STRAIGHT: 0;
        _flag |= IsFOAK(cards) ? (int)CombinationType.FOUR_OF_A_KIND: 0;
        _flag |= IsTOAK(cards) ? (int)CombinationType.THREE_OF_A_KIND : 0;
        _flag |= pairs == 2 ? (int)CombinationType.TWO_PAIR : 0;
        _flag |= pairs == 1 ? (int)CombinationType.PAIR : 0;

        _flag |= _flag == 0 ? (int)CombinationType.HIGH_CARD : 0;

        int isFullHouseFlag = (int)CombinationType.THREE_OF_A_KIND | (int)CombinationType.PAIR;
        _flag = (_flag & isFullHouseFlag) == isFullHouseFlag ? (int)CombinationType.FULL_HOUSE : 0;
        
        int isStraightFlushFlag = (int)CombinationType.STRAIGHT | (int)CombinationType.FLUSH;
        _flag |= (_flag & isStraightFlushFlag) == isStraightFlushFlag ? (int)CombinationType.STRAIGHT_FLUSH : 0;
        
        _flag |= (_flag & (int)CombinationType.STRAIGHT_FLUSH) != 0 && cards[0].Rank == 10 ? (int)CombinationType.ROYAL_FLUSH : 0;
    }


    public static Player Compare(Player p1, Player p2, List<Card> flop) {
        var c1 = new Combination(p1, flop);
        var c2 = new Combination(p2, flop);

        if(c1._flag == c2._flag) {
            return CompareHands(p1, p2);
        } else {
            return c1._flag > c2._flag ? p1 : p2;
        }
    }

    private static Player CompareHands(Player p1, Player p2) {
        static int cmp(Card card) => card.Rank;
        var p1max = p1.Hand.Max(cmp);
        var p2max = p2.Hand.Max(cmp);
        var p1min = p1.Hand.Min(cmp);
        var p2min = p2.Hand.Min(cmp);
        if (p1max == p2max) {
            return p1min > p2min ? p1 : p2;
        }
        return p1min > p2min ? p1 : p2;
    }

    private static List<Card> OrderCards(Player player, List<Card> flop) {
        var result = new List<Card>(flop);
        result.AddRange(player.Hand);
        result = new List<Card>(result.OrderBy(card => card.Rank));
        return result;
    }

    private static bool IsFlush(List<Card> cards) {
        var color = cards[0].Color;
        return cards.All(card => card.Color == color);
    }
    private static bool IsStraight(List<Card> cards) {
        int start = cards[0].Rank;
        bool hasAce = start == 1;
        int following = 1;
        for(int i = 1 ; i < 5 ; i++) {
            if(cards[i].Rank != start + i) {
                break;
            }
            following++;
        }
        return following == 5 || (start == 10 && hasAce);
    }
    private static bool IsFOAK(List<Card> cards) {
        foreach(var card in cards) {
            if(cards.Select(c => c.Rank == card.Rank).Count() == 4) {
                return true;
            }
        }
        return false;
    }
    private static bool IsTOAK(List<Card> cards) {
        foreach(var card in cards) {
            if(cards.Select(c => c.Rank == card.Rank).Count() == 3) {
                return true;
            }
        }
        return false;
    }
    private static int NumberOfPairs(List<Card> cards) {
        int pairs = 0;
        foreach(var card in cards) {
            if(cards.Select(c => c.Rank == card.Rank).Count() == 2) {
                pairs++;
            }
        }
        return pairs/2;
    }

}

[Flags]
public enum CombinationType {
    ROYAL_FLUSH =     0b_1000000000,  
    STRAIGHT_FLUSH =  0b_0100000000,    
    FOUR_OF_A_KIND =  0b_0010000000,
    FULL_HOUSE =      0b_0001000000,
    FLUSH =           0b_0000100000,
    STRAIGHT =        0b_0000010000,
    THREE_OF_A_KIND = 0b_0000001000,
    TWO_PAIR =        0b_0000000100,
    PAIR =            0b_0000000010,
    HIGH_CARD =       0b_0000000001
}
