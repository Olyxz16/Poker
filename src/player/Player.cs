using Poker.Cards;

namespace Poker.Players;

public abstract class Player
{
    
    public int Balance { get; private set; }

    protected readonly List<Card> _hand;
    public IReadOnlyList<Card> Hand { get => _hand; }

    public Player(int balance, List<Card> hand) {
        Balance = balance;
        _hand = hand;
    }

    protected abstract Move ChoseMove(GameState state);
    protected abstract void DisplayErrorMessage(string message);
    public abstract void DisplayBoard(Board board);

    public Move Play(GameState state) {
        Move chosenMove = ChoseMove(state);
        while(!Move.IsValid(state, chosenMove, out string errorMessage))
        {
            DisplayErrorMessage(errorMessage);
            chosenMove = ChoseMove(state);
        }
        if(chosenMove.MoveType == MoveType.BET) {
            Balance -= chosenMove.BetValue;
        }
        return chosenMove;
    }

}
