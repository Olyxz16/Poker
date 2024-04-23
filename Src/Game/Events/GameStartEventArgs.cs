using Poker.Players;

namespace Poker.Events;

public class GameStartEventArgs {
    public List<Player> Players { get; }
    public GameStartEventArgs(List<Player> players) {
        Players = players;
    }
}
