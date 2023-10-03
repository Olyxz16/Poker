using Poker.Cards;
using Poker.Players;

namespace Poker;

public struct GameState
{

    public readonly int Round;
    public readonly int Turn;
    public readonly int Bank;
    public readonly Player Player;
    public IReadOnlyList<int> Bets;
    public IReadOnlyList<Card> Flop;

    public GameState(int round, int turn, int bank, Player player, List<int> bets, List<Card> flop) {
        Round = round;
        Turn = turn;
        Bank = bank;
        Player = player;
        Bets = bets;
        Flop = round switch {
            1 => flop.Take(0).ToList(),
            2 => flop.Take(3).ToList(),
            3 => flop.Take(4).ToList(),
            _ => flop.Take(5).ToList()
        };
    }

}
