using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Poker.Players.Net;

public class Client
{
    
    private const string HOST = Server.HOST;
    private const int PORT = Server.PORT;

    private TcpClient _client;
    private Thread _listeningThread;

    public Client(string host, int port) {
        _client = new TcpClient();
        _client.Connect(IPAddress.Parse(host), port);
        _listeningThread = new Thread(WaitAndAnswer);
        _listeningThread.Start();
        HandleHandshake();
    }
    public Client() : this(HOST, PORT) {}


    private void HandleHandshake() {
        var sendValue = new Protocol()
            .SetInt("X", Console.WindowWidth)
            .SetInt("Y", Console.WindowHeight);
        Send(sendValue);
    }

    private void Send(Protocol value) {
        var stream = _client.GetStream();
        var data = Encoding.UTF8.GetBytes(value.Serialize());
        stream.Write(data, 0, data.Length);
    }
    private Protocol Receive() {
        var stream = _client.GetStream();
        while(!stream.DataAvailable || _client.Available<10);
        var data = new byte[10];
        stream.Read(data, 0, data.Length);

        var sizeData = Encoding.UTF8.GetString(data);
        uint size = uint.Parse(sizeData);

        while(_client.Available != size);
        data = new byte[size];
        stream.Read(data, 0, data.Length);

        Protocol result;
        try {
            result = Protocol.Parse(Encoding.UTF8.GetString(data));
        } catch (Exception) {
            result = new Protocol();
        }
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
            .SetString("STATUS", "OK");
    }



    private void Display(string value) {
        Regex pattern = new("\n");
        value = pattern.Replace(value, "\r");
    }
    private void Clear() {
        Console.Clear();
    }


}
