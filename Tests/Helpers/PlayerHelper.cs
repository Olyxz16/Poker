using Poker.Players;
using Poker.Cards;
using Poker;

namespace Tests.Helpers;

public class PlayerHelper : Player
{

    private static int COUNT = 0;
    private Queue<Move> moves;

    public PlayerHelper(int balance, string name) : base(balance, name)
    {
        moves = new Queue<Move>();
        COUNT++;
    } 
    public PlayerHelper(int balance) : this(balance, "Player"+(COUNT+1)) {}

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

    protected override void DisplayErrorMessage(string message) {}

    protected override void DisplayGameState(GameState state) {}
}