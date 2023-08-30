using Poker.Cards;

namespace Poker;

public class Game
{
    
    Deck deck;

    public Game() {
        deck = Deck.Standard52CardDeck();
        foreach(var card in deck.Cards) {
            Console.WriteLine(card.ToString());
        }
    }


}
