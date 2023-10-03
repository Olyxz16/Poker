using Poker;
using Poker.Cards;
using Poker.Players;

namespace Basic;

public class Program
{
    public static void Main(string[] args) 
    {
        var p1 = new ConsolePlayer(100);
        var p2 = new ConsolePlayer(100);
        var game = new Game(new List<Player> {p1,p2});
        game.Play();
    }
}