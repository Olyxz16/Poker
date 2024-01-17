using Poker.Players;

namespace Poker.Events;

public class PlayerMoveEventArgs {
    public GameState State { get; }
    public Move Move { get; }
    public PlayerMoveEventArgs(GameState state, Move move) {
        State = state;
        Move = move;
    }
}