using Microsoft.Playwright;

namespace Poker.Online.Events;

public class OnPlayerTurnEventArgs {
    public GameState GameState { get; init; }
    public IPage Page { get; init; }
    public OnPlayerTurnEventArgs(GameState state, IPage page) {
        GameState = state;
        Page = page;
    }
}
