using Poker.Cards;
using Poker.Players;

namespace Poker;


public class Program
{
    public static void Main(string[] args) 
    {

        Player player = new ConsolePlayer(100, new List<Card>());

        Console.WriteLine("Turn 1");
        var state = new GameState(1, player, new List<int> { 1, 2 }, new List<Card>());
        player.Play(state);
        
        Console.WriteLine("Turn 2");
        state = new GameState(2, player, new List<int> { 1, 50 }, new List<Card>());
        player.Play(state);

        Console.WriteLine("Turn 3");
        state = new GameState(3, player, new List<int> { 1, 30 }, new List<Card>());
        player.Play(state);

    }
}