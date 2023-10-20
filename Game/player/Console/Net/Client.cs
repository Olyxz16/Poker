using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Poker.Players.Net;

public class Client
{
    
    private const string HOST = "127.0.0.1";
    private const int PORT = 8080;

    private TcpClient _client;
    private Thread _listeningThread;

    public Client(string host, int port) {
        _client = new TcpClient();
        _client.Connect(IPAddress.Parse(host), port);
        HandleHandshake();
        _listeningThread = new Thread(WaitAndAnswer);
        _listeningThread.Start();
    }
    public Client() : this(HOST, PORT) {}


    private void HandleHandshake() {
        var sendValue = new Protocol()
            .SetValue("X", Console.WindowWidth)
            .SetValue("Y", Console.WindowHeight);
        Send(sendValue);
    }

    private void Send(Protocol value) {
        var stream = _client.GetStream();
        var data = Encoding.UTF8.GetBytes(value.Serialize());
        stream.Write(data, 0, data.Length);
    }
    private Protocol Receive() {
        var stream = _client.GetStream();
        while(!stream.DataAvailable);
        var data = new byte[_client.Available];
        stream.Read(data, 0, data.Length);
        var result = Protocol.Parse(Encoding.UTF8.GetString(data));
        return result;
    }


    private void WaitAndAnswer() {
        while(true) {
            var request = Receive();
            var answer = ParseAnswer(request);
            Send(answer);
        }
    }


    private Protocol ParseAnswer(Protocol request) {
        if(request.ContainsKey("DISPLAY_ACTION")) {
            switch(request.GetString("DISPLAY_ACTION")) {
                case "DISPLAY": {
                    var content = request.GetString("DISPLAY_CONTENT");
                    Display(content);
                }; break;
                case "CLEAR": Clear(); break;
            }
        }
        return new Protocol()
            .SetValue("STATUS", "OK");
    }



    private void Display(string value) {
        Console.WriteLine(value);
    }
    private void Clear() {
        Console.Clear();
    }


}
