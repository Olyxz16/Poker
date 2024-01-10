using Poker.Players;
using Poker.Net;

namespace Tests;

public class ConnectionTest 
{

    [Fact]
    public void ConnectLocal() {
        bool connected = false;
        var server = Server.Instance;
        server.OnPlayerAddEvent += (Player p) => {
            connected = true;
        };
        var client = new Client("127.0.0.1", 8080);

        while(!connected);
        Assert.Single(server.Clients);
    }

}