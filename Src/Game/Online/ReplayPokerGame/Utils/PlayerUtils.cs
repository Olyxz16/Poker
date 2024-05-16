using Microsoft.Playwright;
using Poker.Players;
using Poker.Events;

namespace Poker.Online.Utils;

public static class PlayerUtils {
   

    public static async void PlayTurn(object sender, OnPlayerTurnEventArgs e) {
        var page = ReplayPokerGame.Page;
        if(page == null) {
            return;
        }
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
        var button = page.Locator(".BettingControls__action--defensive").First;
        await button.ClickAsync();
        var buttonDisappearTask = Task.Run(async () => {
                var playerLoc = page.Locator(".Seat.Seat--currentUser.Seat--currentPlayer");
                while(await playerLoc.CountAsync() == 1);
                }); 
        var checkDialogBox = Task.Run(async () => {
                var checkButtonLoc = page.Locator(".Button.check").First;
                await checkButtonLoc.ClickAsync();
                });
        await Task.WhenAny(new Task[] { buttonDisappearTask, checkDialogBox });
    }
    private static async Task Check(IPage page) {
        var button = page.Locator(".BettingControls__action--neutral").First;
        await button.ClickAsync();
    }
    private static async Task Bet(IPage page, int value) {
        var button = page.Locator(".BettingControls__action--aggressive").First;
        var inputLoc = page.Locator("NumberInput > input");
        await inputLoc.FillAsync(value.ToString());
        await button.ClickAsync();
    }

}
