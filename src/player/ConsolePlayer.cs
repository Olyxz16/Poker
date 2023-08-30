
using Poker.Cards;

namespace Poker.Players;

public class ConsolePlayer : Player
{

    public ConsolePlayer(int balance, List<Card> hand) : base(balance, hand)
    {
    }

    protected override Move ChoseMove(GameState state)
    {
        Console.WriteLine("Enter input.");
        var input = Console.ReadLine();
        Move move;
        while(!IsValid(input ?? "", out move)) {
            Console.WriteLine("Invalid input.");
            input = Console.ReadLine();
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


    public override void DisplayBoard(Board board)
    {
        Console.WriteLine("");
    }
    protected override void DisplayErrorMessage(string message)
    {
        Console.WriteLine(message);
    }
}
