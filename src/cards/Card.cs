namespace Poker.Cards;

public class Card
{

    public int Rank { get; private set;}
    public CardColor Color { get; private set; }

    public Card(int rank, CardColor color) {
        Rank = rank;
        Color = color;
    }


    public override string ToString() {
        var rankString = Rank switch
        {
            1 => "Ace",
            11 => "Jack",
            12 => "Queen",
            13 => "King",
            _ => Rank.ToString()
        };
        var colorString = Color.ToString();
        return $"{rankString} of {colorString}";
    }


    public static bool operator ==(Card a, Card b)
    {
        return a.Rank == b.Rank;
    }
    public static bool operator !=(Card a, Card b)
    {
        return a.Rank != b.Rank;
    }

    public static bool operator >(Card a, Card b)
    {
        return (a.Rank == 1 && b.Rank != 1) || a.Rank > b.Rank;
    }
    public static bool operator <(Card a, Card b)
    {
        return (b.Rank == 1 && a.Rank != 1) || a.Rank < b.Rank;
    }
    
    public static bool operator >=(Card a, Card b)
    {
        return a.Rank == 1 || a.Rank >= b.Rank;
    }
    public static bool operator <=(Card a, Card b)
    {
        return b.Rank == 1 || a.Rank <= b.Rank;
    }
    
        
}

