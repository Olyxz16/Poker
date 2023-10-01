using GUISharp.Components;
using Poker.Cards;

namespace Poker.Players.UI;

public class CardUI : Component
{

    public CardUI(Card card) : base(5,7) {
        SetBorder('*');
        SetCharAt(GetRankChar(card), 2, 2);
        SetCharAt(GetColChar(card), 2, 4);
    }

    private char GetRankChar(Card card) {
        return card.Rank switch {
            1 => '1',
            2 => '2',
            3 => '3',
            4 => '4',
            5 => '5',
            6 => '6',
            7 => '7',
            8 => '8',
            9 => '9',
            10 => 'X',
            11 => 'V',
            12 => 'D',
            13 => 'R',
            _ => ' '
        };
    }
    private char GetColChar(Card card) {
        return card.Color switch {
            CardColor.CLUB => '♣',
            CardColor.SPADE => '♠',
            CardColor.DIAMOND => '♦',
            CardColor.HEART => '♥',
            _ => ' '
        };
    }

}
