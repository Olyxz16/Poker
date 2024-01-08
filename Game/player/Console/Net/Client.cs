using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

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
        while(!stream.DataAvailable);
        var data = new byte[32];
        stream.Read(data, 0, data.Length);
        int size = int.Parse(Encoding.UTF8.GetString(data));

        while(_client.Available != size);
        data = new byte[size];
        stream.Read(data, 0, data.Length);

        Protocol result = null;
        try {
            result = Protocol.Parse(Encoding.UTF8.GetString(data));
        } catch(Exception e) {
            Console.WriteLine(Encoding.UTF8.GetString(data));
            Console.Read();
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
        //Console.WriteLine(value);
    }
    private void Clear() {
        Console.Clear();
    }


}
