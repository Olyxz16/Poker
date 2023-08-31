using Poker.Cards;
using Poker.Players;

namespace Poker;

public struct GameState
{

    public readonly int Turn;
    public readonly Player Player;
    public IReadOnlyList<int> Bets;
    public IReadOnlyList<Card> Flop;


    public GameState(int turn, Player player, List<int> bets, List<Card> flop) {
        Turn = turn;
        Player = player;
        Bets = bets;
        Flop = flop;
    }

}
