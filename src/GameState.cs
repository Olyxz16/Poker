using Poker.Cards;
using Poker.Players;

namespace Poker;

public struct GameState
{

    public readonly int Turn;
    public readonly Player Player;
    public IReadOnlyList<int> Bets;
    public IReadOnlyList<Card> River;


    public GameState(int turn, Player player, List<int> bets, List<Card> river) {
        Turn = turn;
        Player = player;
        Bets = bets;
        River = river;
    }

}
