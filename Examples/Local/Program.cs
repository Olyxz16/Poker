using Poker;
using Poker.Players;

namespace Local;

public class Program
{
    public static void Main(string[] args) 
    {
        var p1 = new LocalConsolePlayer(100);
        var p2 = new LocalConsolePlayer(100);
        var game = new Game(new List<Player> {p1,p2});
        game.Play();
    }
}