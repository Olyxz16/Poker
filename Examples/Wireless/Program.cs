using Poker.Players.Net;

public class Program
{
    public static void Main(string[] args) 
    {
        
        var server = Server.Instance;
        var client = new Client();

        var player = server._players.First().Key;
        var req = new Protocol()
            .SetValue("test", 0); 

        server.SendAndWaitForAnswer(player, req);
        while(server.IsRunning);

    }
}