using Poker.Players;
using Poker.Players.Net;

namespace Poker;

public class Lobby
{
    
    private readonly int _balance;
    private readonly List<Player> _players;
    private readonly Server server;

    public Lobby() : this(Game.DEFAULT_BALANCE) {}
    public Lobby(int balance) {
        _balance = balance;
        _players = new List<Player>();
        server = Server.Instance;
        server.OnPlayerAddEvent += AddPlayer;
    }

    public List<Player> WaitForPlayers() {
        var key = Console.ReadKey();
        while(key.Key != ConsoleKey.Enter) {
            Console.WriteLine("Press Enter to stop.");
            key = Console.ReadKey();
        }
        Console.WriteLine($"Lobby closed with ${_players.Count} players;");
        return _players;
    }



    private void AddPlayer(Player player) {
        _players.Add(player);
        Console.WriteLine("Player added");
    }


    

}
