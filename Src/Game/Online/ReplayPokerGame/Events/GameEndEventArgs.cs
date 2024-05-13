using Poker.Players;

namespace Poker.Online.Events;

public class GameEndEventArgs {
    public Player? Winner { get; init; }
    public int Money { get; init; }
    public GameEndEventArgs(Player? winner, int money) {
        Winner = winner;
        Money = money;
    }
}
