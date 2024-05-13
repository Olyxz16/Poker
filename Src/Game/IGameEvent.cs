using Poker.Events;

namespace Poker;

public interface IGameEvents {
    
    public delegate void GameStartEventHandler(object sender, GameStartEventArgs e);
    public event GameStartEventHandler? GameStartEvent;
        
    public delegate void GameEndEventHandler(object sender, GameEndEventArgs e);
    public event GameEndEventHandler? GameEndEvent;
    
    public delegate void OnPlayerTurnEventHandler(object sender, OnPlayerTurnEventArgs e);
    public event OnPlayerTurnEventHandler? OnPlayerTurnEvent;

}
