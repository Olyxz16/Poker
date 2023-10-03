using Poker.Cards;

namespace Poker.Players;

public abstract class Player
{

    protected static int PLAYER_COUNT = 1;
    
    public int Balance;
    public string Name;

    // Protect this to avoid being able to have more than 2 cards.
    private List<Card> _hand;
    public IReadOnlyList<Card> Hand => _hand;

    public Player(int balance): this(balance, RandomName()) {}
    public Player(int balance, string name) {
        Balance = balance;
        Name = name;
        _hand = new List<Card>(2);
    }

    protected abstract Move ChoseMove(GameState state);
    protected abstract void DisplayGameState(GameState state);
    protected abstract void DisplayErrorMessage(string message);
    

    public Move Play(GameState state) {
        DisplayGameState(state);
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

    private static string RandomName() {
        return $"Player-{new Random().Next()}";
    }

}
