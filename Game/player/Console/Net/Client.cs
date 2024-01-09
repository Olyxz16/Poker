using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Poker.Players.Net;

public class Client : TCPCommunicator
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
        Send(_client, sendValue);
    }


    private void WaitAndAnswer() {
        while(true) {
            var request = Receive(_client);
            var answer = ParseAnswer(request);
            Send(_client, answer);
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
