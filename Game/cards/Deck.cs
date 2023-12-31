namespace Poker.Cards;

public class Deck
{
    
    private List<Card> _cards;
    public IReadOnlyList<Card> Cards { get => _cards; }
 
    private Deck() {
        _cards = new List<Card>();
    }


    public static Deck Standard52CardDeck() {
        var deck = new Deck();

        deck._cards.AddRange(GenerateSet(CardColor.CLUB));
        deck._cards.AddRange(GenerateSet(CardColor.DIAMOND));
        deck._cards.AddRange(GenerateSet(CardColor.HEART));
        deck._cards.AddRange(GenerateSet(CardColor.SPADE));
        
        return deck;

        static List<Card> GenerateSet(CardColor color) {
            var cards = new List<Card>(13);
            for(int i = 1 ; i <= 13; i++) {
                cards.Add(new Card(i, color));
            }
            return cards;
        }
    }


    public List<Card> Draw(int number) {
        var cards = new List<Card>();
        for(int i = 0 ; i < number ; i++) {
            cards.Add(_cards[0]);
            _cards.RemoveAt(0);
        }
        return cards;
    }

    // To be recrated with better random function
    public void Shuffle() {
        var rnd = new Random();
        _cards = new List<Card>(_cards.OrderBy(item => rnd.Next()));
    }


}
