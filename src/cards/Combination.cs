using Poker.Players;

namespace Poker.Cards;

public struct Combination
{

    private int _flag;

    private Card _primaryCard;
    private Card _secondaryCard;

    private Card _best_triple_rank;
    private Card _best_pair_rank;

    private Card _low_straight_card;
    private Card _flush_color;
    private List<Card> _flush_cards;


    private Combination(Player player, List<Card> flop) {
        _flag = 0;
        _flush_cards = new List<Card>();
        var cards = OrderCards(player, flop);
        SetFlushFlag(cards);
        SetStraightFlag(cards);
        SetFourOfAKindFlag(cards);
        SetThreeOfAKindFlag(cards);
        SetPairsFlag(cards);

        SetHighCardFlag(cards);

        SetFullHouseFlag(cards);
        SetStraightFlushFlag(cards);
        SetRoyalFlushFlag(cards);
    }


    public static Player? Compare(Player p1, Player p2, List<Card> flop) {
        var c1 = new Combination(p1, flop);
        var c2 = new Combination(p2, flop);

        if(c1._flag != c2._flag) {
            return c1._flag > c2._flag ? p1 : p2;
        }

        switch(c1._flag) {
            case >= (int)CombinationType.STRAIGHT_FLUSH : {
                if(c1._low_straight_card == c2._low_straight_card) {
                    return null;
                }
                return c1._low_straight_card > c2._low_straight_card ? p1 : p2;
            };
            case >= (int)CombinationType.FOUR_OF_A_KIND : {
                return c1._primaryCard > c2._primaryCard ? p1 : p2;
            };
            case >= (int)CombinationType.FULL_HOUSE : {
                return c1._primaryCard > c2._primaryCard ? p1 : p2;
            };
            case >= (int)CombinationType.FLUSH : {
                for(int i = 0 ; i < 5 ; i++) {
                    if(c1._flush_cards[i] == c2._flush_cards[i]) {
                        continue;
                    }
                    return c1._flush_cards[i] > c2._flush_cards[i] ? p1 : p2;
                }
                return null;
            };
            case >= (int)CombinationType.STRAIGHT : {
                if(c1._low_straight_card == c2._low_straight_card) {
                    return null;
                }
                return c1._low_straight_card > c2._low_straight_card ? p1 : p2;
            };
            case >= (int)CombinationType.THREE_OF_A_KIND : {
                return c1._primaryCard > c2._primaryCard ? p1 : p2;
            };
            case >= (int)CombinationType.TWO_PAIR : {
                if(c1._primaryCard == c2._primaryCard) {
                    if(c1._secondaryCard == c2._secondaryCard) {
                        return null;
                    }
                    return c1._secondaryCard > c2._secondaryCard ? p1 : p2;
                }
                return c1._primaryCard > c2._primaryCard ? p1 : p2;
            };
            case >= (int)CombinationType.PAIR : {
                if(c1._primaryCard == c2._primaryCard) {
                    var hc1 = c1._primaryCard == p1.Hand[0] ? p1.Hand[0] : p1.Hand[1];
                    var hc2 = c2._primaryCard == p2.Hand[0] ? p2.Hand[0] : p2.Hand[1];
                    return hc1 > hc2 ? p1 : p2;
                }
                return c1._primaryCard > c2._primaryCard ? p1 : p2;
            };
            case >= (int)CombinationType.HIGH_CARD : {
                (var p1max, var p1min) = p1.Hand[0] >= p1.Hand[1] ? (p1.Hand[0], p1.Hand[1]) : (p1.Hand[1], p1.Hand[0]);
                (var p2max, var p2min) = p2.Hand[0] >= p2.Hand[1] ? (p2.Hand[0], p2.Hand[1]) : (p2.Hand[1], p2.Hand[0]);
                if(p1max == p2max) {
                    return p1min > p2min ? p1 : p2;
                }
                return p1max > p2max ? p1 : p2;
            };
            default: return null;
        }

    }


    private static List<Card> OrderCards(Player player, List<Card> flop) {
        var result = new List<Card>(flop);
        result.AddRange(player.Hand);
        result = new List<Card>(result.OrderBy(card => card.Rank));
        return result;
    }


    private void SetFlushFlag(List<Card> cards) {
        if(_flag > (int)CombinationType.FLUSH) {
            return;
        }
        foreach(var card in cards) {
            var color = cards[0].Color;
            var flush_cards = cards.FindAll(card => card.Color == color);
            if(flush_cards.Count >= 5) {
                _flag |= (int)CombinationType.FLUSH;
                _flush_color = card;
                _flush_cards = (List<Card>)flush_cards.OrderByDescending(card => card.Rank);
                return;
            }
        }
    }
    private void SetStraightFlag(List<Card> cards) {
        if(_flag > (int)CombinationType.STRAIGHT) {
            return;
        }
        for(int startIndex = 0 ; startIndex < 3 ; startIndex++) {
            var pivotCard = cards[startIndex];
            bool isStraight = true;
            for(int cardIndex = 1 ; cardIndex < 5 ; cardIndex++) {
                if(cards[startIndex+cardIndex].Rank != pivotCard.Rank+cardIndex) {
                    isStraight = false;
                    break;
                }
            }
            if(isStraight) {
                _flag |= (int)CombinationType.STRAIGHT;
                _low_straight_card = pivotCard;
            }
        }
    }
    private void SetFourOfAKindFlag(List<Card> cards) {
        if(_flag > (int)CombinationType.FOUR_OF_A_KIND) {
            return;
        }
        foreach(var card in cards) {
            if(cards.FindAll(c => c == card).Count == 4) {
                _flag |= (int)CombinationType.FOUR_OF_A_KIND;
                _primaryCard = card;
                return;
            }
        }
    }
    private void SetThreeOfAKindFlag(List<Card> cards) {
        if(_flag > (int)CombinationType.THREE_OF_A_KIND) {
            return;
        }
        bool hasOneTriplet = false;
        foreach(var card in cards) {
            if(cards.FindAll(c => c == card).Count == 3) {
                _flag |= (int)CombinationType.THREE_OF_A_KIND;
                if(hasOneTriplet && card != _primaryCard) {
                    if(card > _primaryCard) {
                        _secondaryCard = _primaryCard;
                        _primaryCard = card;
                    } else {
                        _secondaryCard = card;
                    }
                } else {
                    hasOneTriplet = true;
                    _primaryCard = card;
                    _secondaryCard = new();
                }
            }
        }
        _best_triple_rank = _primaryCard;
    }
    private void SetPairsFlag(List<Card> cards) {
        if(_flag > (int)CombinationType.PAIR) {
            return;
        }
        bool isSecond = false;
        foreach(var card in cards) {
            if(cards.FindAll(c => c == card).Count == 2) {
                _flag |= (int)CombinationType.PAIR;
                if(isSecond && card != _primaryCard) {
                    _flag |= (int)CombinationType.TWO_PAIR;
                    if(card > _primaryCard) {
                        _secondaryCard = _primaryCard;
                        _primaryCard = card;
                    } else if(card > _secondaryCard) {
                        _secondaryCard = card;
                    }
                } else {
                    isSecond = true;
                    _primaryCard = card;
                    _secondaryCard = new();
                }
            }
        }
        _best_pair_rank = _primaryCard;
    }
    
    private void SetHighCardFlag(List<Card> cards) {
        if(_flag > (int)CombinationType.HIGH_CARD) {
            return;
        }
        _flag = (int)CombinationType.HIGH_CARD;
        int len = cards.Count;
        _primaryCard = cards[len-1];
        _secondaryCard = cards[len-2];
    }
    
    private void SetFullHouseFlag(List<Card> cards) {
        var full_house_flag = (int)CombinationType.THREE_OF_A_KIND | (int)CombinationType.PAIR;
        if((_flag & full_house_flag) != (int)CombinationType.FULL_HOUSE) {
            return;
        }
        _flag |= (int)CombinationType.FULL_HOUSE;
        _primaryCard = _best_triple_rank;
        _secondaryCard = _best_pair_rank;
    }
    private void SetStraightFlushFlag(List<Card> cards) {
        var straight_flush_flag = (int)CombinationType.STRAIGHT | (int)CombinationType.FLUSH;
        if((_flag & straight_flush_flag) != (int)CombinationType.STRAIGHT_FLUSH) {
            return;
        }
        _flag |= (int)CombinationType.STRAIGHT_FLUSH;
        _primaryCard = _low_straight_card;
        _secondaryCard = _flush_color;
    }
    private void SetRoyalFlushFlag(List<Card> cards) {
        if((_flag & (int)CombinationType.STRAIGHT_FLUSH) == 0 || _low_straight_card.Rank != 10) {
            return;
        }
        _flag |= (int)CombinationType.ROYAL_FLUSH;
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
