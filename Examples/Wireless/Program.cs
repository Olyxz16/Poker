using Poker;
using Poker.Net;

public class Program
{

    public static void Main(string[] args) 
    {

        if(args.Contains("--host")) {
            HostGame();
        }

        if(args.Contains("--client")) {
            string ip = "";
            int port = 0;
            try {
                int index = Array.IndexOf(args, "--client");
                var address = args[index+1].Split(':');
                ip = address[0];
                port = int.Parse(address[1]);
            } catch(Exception) {
                throw;
            } finally {
                _ = new Client(ip, port);
            }
        }
        
    }


    private static void HostGame() {
        var lobby = new Lobby(500);
        var players = lobby.WaitForPlayers();
        var game = new Game(players);
        game.Play();
    }

}