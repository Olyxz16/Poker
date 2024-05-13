using Microsoft.Playwright;
using Poker.Online.Events;
using Poker.Players;

namespace Poker.Online.Utils;

public static class PlayerUtils {
   

    public static async void PlayTurn(object sender, OnPlayerTurnEventArgs e) {
        var page = e.Page;
        var state = e.GameState; 
        var player = state.Player;
        
        var move = player.Play(state);
        
        if(move.MoveType == MoveType.FOLD) {
            await Fold(page);
            return;
        }
        var value = move.BetValue;
        if(value == 0) {
            await Check(page); 
        } else {
            await Bet(page, value);
        }

    }

    private static async Task Fold(IPage page) {
        
    }
    private static async Task Check(IPage page) {

    }
    private static async Task Bet(IPage page, int value) {
        
    }

}
