using GUISharp;
using Poker.Players.Net;

namespace Poker.Players;

public class RemoteConsolePlayer : ConsolePlayer, IDisplayable
{

    private Server _server;
    public int NetID => _netID;
    private readonly int _netID;

    public RemoteConsolePlayer(int balance, int id, int x, int y) : base(balance)
    {
        frame = new Frame(this, x, y);
        _server = Server.Instance;
        _netID = id;
        DisplayWaitingScreen();
    }

    public override void Clear()
    { 
        var req = new Protocol()
            .SetString("DISPLAY_ACTION", "CLEAR");
        _server.SendAndWaitForAnswer(this, req);
    }

    public override (int x, int y) GetSize()
    {
        throw new NotImplementedException();
    }

    public override (int x, int y) GetCursorPosition()
    {
        throw new NotImplementedException();
    }

    public override string Prompt()
    {
        var req = new Protocol()
            .SetString("GET", "PROMPT");
        var answer = _server.SendAndWaitForAnswer(this, req);
        if(!answer.Success) {
            return "";
        }
        return answer.GetString("VALUE");
    }

    public override void SetCursorPosition(int x, int y)
    {
        throw new NotImplementedException();
    }

    public override void Write(string val)
    {
        var req = new Protocol()
            .SetString("DISPLAY_ACTION", "DISPLAY")
            .SetString("DISPLAY_CONTENT", val);
        _server.SendAndWaitForAnswer(this, req);
    }
}
