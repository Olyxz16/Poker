using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Poker.Players.Net;

public sealed class Server
{

    internal const string HOST = "127.0.0.1";
    internal const int PORT = 8080;
    
    public static Server Instance { get {
        _instance ??= new Server();
        return _instance;
    } }
    private static Server? _instance;

    public bool IsRunning => _listeningThread.ThreadState == ThreadState.Running;

    private TcpListener _listener;
    private Thread _listeningThread;
    private bool _listening;
    public delegate void OnPlayerAdd(Player player);
    public event OnPlayerAdd? OnPlayerAddEvent;

    private Dictionary<int, TcpClient> _clients;
    public IReadOnlyDictionary<int, TcpClient> Clients => _clients;

    private Server() { 
        _listening = true;
        _listener = new TcpListener(IPAddress.Any, PORT);
        _clients = new Dictionary<int, TcpClient>();

        _listeningThread = new Thread(Listen);
        _listeningThread.Start();

        Console.WriteLine("Listening ...");
    }

    private void Listen() 
    {
        _listener.Start();
        while(_listening) {
            TcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine("Player connecting ...");
            HandleHandshake(client);
        }
    }


    public void SendAndWaitForAnswer(RemoteConsolePlayer player, Protocol request) {
        var targetClient = _clients[player.NetID];
        Send(targetClient, request);
        var answer = Receive(targetClient);
    }
     
     private void Send(TcpClient client, Protocol value) {
        var stream = client.GetStream();
        var data = Encoding.UTF8.GetBytes(value.Serialize());
        var dataSize = GetDataSizeString(data);
        stream.Write(dataSize);
        stream.Write(data, 0, data.Length);
    }
    private static byte[] GetDataSizeString(byte[] data) {
        var len = data.Length;
        var result = len.ToString().PadLeft(10, '0');
        return Encoding.UTF8.GetBytes(result);
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
        int id = GenerateID();
        _clients.Add(id,client);
        var player = new RemoteConsolePlayer(100, id, sizeX, sizeY);
        OnPlayerAddEvent?.Invoke(player);
    }


    private int GenerateID() {
        int result;
        do {
            result = new Random().Next();
        } while(_clients.ContainsKey(result));
        return result;
    }


}
