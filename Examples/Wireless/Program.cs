using System.Runtime.CompilerServices;
using Poker;
using Poker.Players;
using Poker.Players.Net;

public class Program
{

    public static void Main(string[] args) 
    {
        
        var players = new List<Player>();

        var server = Server.Instance;
        server.OnPlayerAddEvent += AddAndClear;
        var client = new Client();

        while(server.IsRunning);

        void AddAndClear(Player player) {
            players.Add(player);
            (player as RemoteConsolePlayer).Clear();
        }

    }

}