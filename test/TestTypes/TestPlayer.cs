using Poker.Players;
using Poker.Cards;

namespace Poker.Testing;

public class TestPlayer : Player
{

    private Queue<Move> moves;

    public TestPlayer(int balance) : base(balance)
    {
        moves = new Queue<Move>();
    }

    public void AddMove(Move move) {
        moves.Enqueue(move);
    }

    public void DrawHand(Card c1, Card c2) {
        DrawHand(new List<Card> {c1, c2});
    }

    protected override Move ChoseMove(GameState state)
    {
        return moves.Dequeue();
    }

    protected override void DisplayErrorMessage(string message)
    {}
    
}