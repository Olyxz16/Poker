using Poker.Cards;

namespace Poker.Online.Utils;

public struct CardSVGPair {
    public Card? Card { get; init; }
    public string SVG { get; init; }
    public string ImagePath { get; init; }
    public CardSVGPair(string svg, string path) {
        Card = null;
        SVG = svg;
        ImagePath = path;
    }
    public CardSVGPair(Card card, string svg, string path) {
        Card = card;
        SVG = svg;
        ImagePath = path;
    }
}
