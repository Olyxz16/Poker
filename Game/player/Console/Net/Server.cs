using System.Data.SqlTypes;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Poker.Players.Net;

public sealed class Server
{

    private const string HOST = "127.0.0.1";
    private const int PORT = 8080;
    
    public static Server Instance { get {
        _instance ??= new Server();
        return _instance;
    } }
    private static Server? _instance;

    public bool IsRunning => _listeningThread.ThreadState == ThreadState.Running;

    private TcpListener _listener;
    private Thread _listeningThread;
    private bool _listening;

    public Dictionary<RemoteConsolePlayer, int> _players;
    private Dictionary<int, TcpClient> _clients;


    private Server() { 

        _listening = true;
        _listener = new TcpListener(IPAddress.Any, PORT);
        _players = new Dictionary<RemoteConsolePlayer, int>();
        _clients = new Dictionary<int, TcpClient>();

        _listeningThread = new Thread(Listen);
        _listeningThread.Start();

        Console.WriteLine("Server running");
        
    }

    private void Listen() 
    {
        _listener.Start();
        while(_listening) {
            TcpClient client = _listener.AcceptTcpClient();
            HandleHandshake(client);
        }
    }


    public void SendAndWaitForAnswer(RemoteConsolePlayer player, Protocol request) {
        var targetClient = _clients[_players[player]];
        Send(targetClient, request);
        var answer = Receive(targetClient);
    }

     private void Send(TcpClient client, Protocol value) {
        var stream = client.GetStream();
        var data = Encoding.UTF8.GetBytes(value.Serialize());
        stream.Write(data, 0, data.Length);
    }
    private Protocol Receive(TcpClient client) {
        var stream = client.GetStream();
        while(!stream.DataAvailable);
        var data = new byte[client.Available];
        stream.Read(data, 0, data.Length);
        var result = Protocol.Parse(Encoding.UTF8.GetString(data));
        return result;
    }


    private void HandleHandshake(TcpClient client) {
        var stream = client.GetStream();
        while(!stream.DataAvailable);
        var bytes = new byte[client.Available];
        stream.Read(bytes, 0, bytes.Length);
        var data = Protocol.Parse(Encoding.UTF8.GetString(bytes));
        
        int sizeX = data.GetInt("X");
        int sizeY = data.GetInt("Y");
        var player = new RemoteConsolePlayer(100, sizeX, sizeY);

        int id = GenerateID();
        var answer = new Protocol()
            .SetValue("ID", id);
        stream.Write(Encoding.UTF8.GetBytes(answer.Serialize()));

        _players.Add(player,id);
        _clients.Add(id,client);
    }


    private int GenerateID() {
        int result;
        do {
            result = new Random().Next();
        } while(_players.ContainsValue(result));
        return result;
    }


}
