using Poker.Players;

namespace Poker;


public class Program
{
    public static void Main(string[] args) 
    {

        var player = new ConsolePlayer(100);

        var game = new Game(new List<Player> { player });
        game.Play();

    }
}