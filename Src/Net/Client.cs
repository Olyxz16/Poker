using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Poker.Net;

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
            return ParseDisplayAction(request);
        }
        if(request.ContainsKey("GET")) {
            return ParseGetRequest(request);
        }
        if(request.ContainsKey("SET")) {
            return ParseSetRequest(request);
        }
        return new Protocol();
    }

    private Protocol ParseGetRequest(Protocol request) {
        switch(request.GetString("GET")) {
            case "SIZE": return ParseSize();
            case "CURSOR": return ParseCursorPos();
            case "PROMPT": return ParsePrompt();
            default: break;
        }
        return new Protocol();
    }
    private static Protocol ParseSize() {
        return new Protocol()
            .SetInt("X", Console.WindowWidth)
            .SetInt("Y", Console.WindowHeight);
    }
    private static Protocol ParseCursorPos() {
        (int x, int y) = Console.GetCursorPosition();
        return new Protocol()
            .SetInt("X", x)
            .SetInt("Y", y);
    }
    private static Protocol ParsePrompt() {
        var value = Console.ReadLine() ?? "";
        return new Protocol()
            .SetString("VALUE", value);
    }


    private Protocol ParseSetRequest(Protocol request) {
        switch(request.GetString("SET")) {
            case "CURSOR": return ParseSetCursor(request);
            default: break;
        }
        return new Protocol();
    }
    private Protocol ParseSetCursor(Protocol request) {
        int x = request.GetInt("X");
        int y = request.GetInt("Y");
        Console.SetCursorPosition(x, y);
        return new Protocol();
    }


    private Protocol ParseDisplayAction(Protocol request) {
        switch(request.GetString("DISPLAY_ACTION")) {
            case "DISPLAY": {
                var content = request.GetString("DISPLAY_CONTENT");
                Display(content);
            }; break;
            case "CLEAR": Clear(); break;
        }
        return new Protocol();
    }



    private void Display(string value) {
        Regex pattern = new("\n");
        value = pattern.Replace(value, "");
        Console.Write(value);
    }
    private void Clear() {
        Console.Clear();
    }


}
