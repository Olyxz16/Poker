namespace Poker.Online.Events;

public class OnPlayerTurnEventArgs {
    public GameState GameState { get; init; }
    public OnPlayerTurnEventArgs(GameState state) {
        GameState = state;
    }
}
