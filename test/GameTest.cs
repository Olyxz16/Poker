using Poker.Cards;
using Poker.Players;

namespace Poker.Testing;

public class GameTest {

    //[Fact]
    public void Game1() {

        // Q, 8 // 7, 2 // 10, A, 2, V, K
        var player1 = new TestPlayer(100);
        player1.DrawHand(new(12, CardColor.CLUB), new(8, CardColor.DIAMOND));
        var player2 = new TestPlayer(100);
        player2.DrawHand(new(7, CardColor.DIAMOND), new(2, CardColor.SPADE));

        var flop = new List<Card> {
            new(10, CardColor.CLUB),
            new(1, CardColor.CLUB),
            new(2, CardColor.DIAMOND),
            new(11, CardColor.CLUB),
            new(13, CardColor.CLUB)
        };
        var game = new TestGame(new List<Player> { player1, player2 }, flop);
        
        //       TURN 1
        player1.AddMove(Move.Bet(10));
        player2.AddMove(Move.Bet(20));
        //       TURN 2
        player1.AddMove(Move.Bet(90));
        player2.AddMove(Move.Bet(80));
        //       TURN 3
        player1.AddMove(Move.Bet(20));
        player2.AddMove(Move.Bet(20));
        //       TURN 4
        player1.AddMove(Move.Bet(20));
        player2.AddMove(Move.Bet(20));



        var winner = game.Play();
        Assert.Same(winner, player1);

    }
    
}