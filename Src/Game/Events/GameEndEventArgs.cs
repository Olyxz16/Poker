namespace Poker.Events;

public class GameEndEventArgs {
    public GameEndState State { get; }
    public GameEndEventArgs(GameEndState state) {
        State = state;
    }
}