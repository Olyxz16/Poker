using Poker.Cards;

namespace Poker.Players;

public abstract class Player
{
    
    public int Balance;

    // Protect this to avoid being able to have more than 2 cards.
    protected List<Card> _hand;
    public IReadOnlyList<Card> Hand { get => _hand; }

    public Player(int balance) {
        Balance = balance;
        _hand = new List<Card>(2);
    }

    protected abstract Move ChoseMove(GameState state);
    protected abstract void DisplayErrorMessage(string message);
    

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

    protected internal void DrawHand(List<Card> cards) {
        if(cards.Count != 2) {
            throw new ArgumentException("Too much cards drawn");
        }
        _hand = cards;
    }

}
