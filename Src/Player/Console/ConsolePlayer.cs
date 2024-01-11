using GUISharp;
using GUISharp.Components;
using Poker.Players.UI;

namespace Poker.Players;

public abstract class ConsolePlayer : Player, IDisplayable
{

    protected Frame frame;
    protected readonly TextField errorField;

    public ConsolePlayer(int balance) : base(balance) {
        frame = Frame.Empty();
        Name = $"Player-{PLAYER_COUNT++}";
        errorField = new TextField("");
    }

    protected override Move ChoseMove(GameState state)
    {
        var inputField = new InputField(frame, "Enter input : ");
        frame.AddComponent(inputField, frame.Center.X - 7, frame.Center.Y + 5, true);
        var input = inputField.Prompt();
        Move move;
        while(!IsValid(input ?? "", out move)) {
            errorField.SetText("Invalid input.");
            frame.AddComponent(errorField, frame.Center.X - 7, frame.Center.Y + 6, true);
            input = inputField.Prompt();
        }
        return move;
    }

    private static bool IsValid(string input, out Move move) {
        var pars = (input ?? "").ToLower().Split(" ");
        if(pars.Length == 0) {
            move = Move.Fold();
            return false;
        }
        if(pars[0] == "fold") {
            move = Move.Fold();
            return true;
        }
        if(pars[0] == "bet" && pars.Length > 1) {
            if (int.TryParse(pars[1], out int numValue)) {
                move = Move.Bet(numValue);
                return true;
            }
        }
        move = Move.Fold();
        return false;
    }


    protected override void DisplayGameState(GameState state) {
        frame.ClearComponents();
        frame.SetBorder('*');
        
        CreateHandUI();
        CreateTitleUI(state);
        CreateFlopUI(state);
        CreateBetsUI(state);

        var nameUI = new TextField(Name);
        frame.AddComponent(nameUI, 4, 2);

        var smallBlindUI = new TextField($"Small blind : {Game.SMALL_BLIND}");
        frame.AddComponent(smallBlindUI, 4, 5);
        var bigBlindUI = new TextField($"Big blind : {Game.BIG_BLIND}");
        frame.AddComponent(bigBlindUI, 4, 7);

        string balanceText = $"Balance: {Balance}";
        var balanceUI = new TextField(balanceText);
        frame.AddComponent(balanceUI, frame.SizeX - balanceText.Length - 4, frame.SizeY - 3);

        string bankText = $"Bank: {state.Bank}";
        var bankUI = new TextField(bankText);
        frame.AddComponent(bankUI, frame.Center.X - bankUI.SizeX/2, frame.Center.Y + 2);

        frame.Display();
    }

    protected virtual void DisplayWaitingScreen() {
        frame.ClearComponents();
        frame.SetBorder('*');

        var text = new TextField("Waiting for players to join...");
        int xPos = frame.Center.X - text.SizeX/2;
        int yPos = frame.Center.Y;
        frame.AddComponent(text, xPos, yPos);

        frame.Display();
    }

    private void CreateTitleUI(GameState state) {
        var text = state.Player == this ? 
            "Your turn !" :
            $"{state.Player.Name}'s turn.";
        var titleUI = new TextField(text);
        frame.AddComponent(titleUI, frame.Center.X - titleUI.SizeX/2, 5);
    }
    private void CreateHandUI() {
        var HandCard1UI = new CardUI(Hand[0]);
        frame.AddComponent(HandCard1UI, frame.Center.X - HandCard1UI.SizeX/2 - 2, frame.Center.Y + 10);
        var HandCard2UI = new CardUI(Hand[1]);
        frame.AddComponent(HandCard2UI, frame.Center.X - HandCard2UI.SizeX/2 + 6, frame.Center.Y + 10);
    }
    private void CreateFlopUI(GameState state) {
        int xOff = frame.Center.X;
        int yOff = frame.Center.Y - 6;
        int xMargin = 7;
        for(int i = 0 ; i < 5 ; i++) {
            CardUI ui;
            if(i < state.Flop.Count) {
                ui = new CardUI(state.Flop[i]);
            } else {
                ui = CardUI.UnrevealedCard();
            }
            int xPos = xOff - ui.PosX/2 + (i-2) * xMargin;
            frame.AddComponent(ui, xPos, yOff);
        }
    }
    private void CreateBetsUI(GameState state) {
        var bets = state.Bets;
        int count = bets.Count;
        int xOff = frame.Center.X;
        int yOff = frame.Center.Y - 10;
        int xMargin = 10;
        int index = 0;
        foreach(var bet in bets) {
            index++;
            var nameField = new TextField(bet.Key);
            var betField = new TextField(bet.Value.ToString());
            int xPos = xOff - nameField.PosX/2 + (int)((index-(0.5f+count/2f)) * xMargin);
            frame.AddComponent(nameField, xPos, yOff);
            frame.AddComponent(betField, xPos, yOff+1);
        }
    }
    

    protected override void DisplayErrorMessage(string message)
    {
        errorField.SetText(message);
        frame.AddComponent(errorField, frame.Center.X - 7, frame.Center.Y + 6, true);
    }


    public override async Task<bool> ConfirmGameEnd(GameEndState state)
    {
        await Task.Run(() => {
            frame.ClearComponents();
            frame.SetBorder('*');

            if(state.IsDraw) {
                var textField = new TextField("Draw");
                frame.AddComponent(textField, frame.Center.X - textField.SizeX/2, frame.Center.Y + 2);

            } else {
                var textField = new TextField($"{state.Winner.Name} won.");
                frame.AddComponent(textField, frame.Center.X - textField.SizeX/2, frame.Center.Y + 2);
            }

            var confirmField = new InputField(this.frame, "Press anything to play again.");
            frame.AddComponent(confirmField, frame.Center.X - confirmField.SizeX/2, frame.Center.Y + 5);

            frame.Display();
            frame.Prompt();
        });
        return true;
    }

    public abstract void Write(string val);
    public abstract void Clear();
    public abstract void SetCursorPosition(int x, int y);
    public abstract string Prompt();
    public abstract (int x, int y) GetSize();
    public abstract (int x, int y) GetCursorPosition();
    
}
