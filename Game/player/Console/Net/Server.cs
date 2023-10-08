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


    private TcpListener _listener;
    private bool _listening;

    private List<TcpClient> _clients;


    private Server() { 

        _listening = true;
        _listener = new TcpListener(IPAddress.Any, PORT);
        _clients = new List<TcpClient>();

        var listeningThread = new Thread(Listen);
        listeningThread.Start();

        while(_listening);
        
    }

    private void Listen() 
    {
        _listener.Start();
        while(_listening) {
            TcpClient client = _listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            while(client.Available < 3);
            byte[] bytes = new byte[client.Available];
            stream.Read(bytes, 0, bytes.Length);
            var data = Encoding.UTF8.GetString(bytes);
            if (new System.Text.RegularExpressions.Regex("^GET").IsMatch(data))
            {
                var response = GetConfirmation(data);
                stream.Write(response, 0, response.Length);
            }
            _clients.Add(client);
        }
    }

    
    private static byte[] GetConfirmation(string header) {
        
        const string eol = "\r\n"; // HTTP/1.1 defines the sequence CR LF as the end-of-line marker

        byte[] response = Encoding.UTF8.GetBytes("HTTP/1.1 101 Switching Protocols" + eol
            + "Connection: Upgrade" + eol
            + "Upgrade: websocket" + eol
            + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                System.Security.Cryptography.SHA1.Create().ComputeHash(
                    Encoding.UTF8.GetBytes(
                        new System.Text.RegularExpressions.Regex("Sec-WebSocket-Key: (.*)").Match(header).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                    )
                )
            ) + eol
            + eol);

        return response;
    
    }




}
