using GUISharp;
using GUISharp.Components;
using Poker.Players.UI;

namespace Poker.Players;

public class ConsolePlayer : Player
{

    private readonly Frame frame;

    public ConsolePlayer(int balance) : base(balance) {
        frame = Frame.GetCurrentFrame();
    }

    protected override Move ChoseMove(GameState state)
    {
        var inputField = new InputField("Enter input : ");
        frame.AddComponent(inputField, 10, 15, true);
        var input = inputField.Prompt();
        Move move;
        while(!IsValid(input ?? "", out move)) {
            var textField = new TextField("Invalid input.");
            frame.AddComponent(textField, 10, 23, true);
            input = inputField.Prompt();
        }
        return move;
    }

    private bool IsValid(string input, out Move move) {
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
        frame.SetBorder('*');
        
        var HandCard1UI = new CardUI(Hand[0]);
        frame.AddComponent(HandCard1UI, 5, 20);
        var HandCard2UI = new CardUI(Hand[1]);
        frame.AddComponent(HandCard2UI, 15, 20);

        frame.Display();
    }

    

    protected override void DisplayErrorMessage(string message)
    {
        Console.WriteLine(message);
    }

    
    
}
