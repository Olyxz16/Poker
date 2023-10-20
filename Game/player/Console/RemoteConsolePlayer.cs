using GUISharp;
using GUISharp.Components;
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
        var component = new TextField("Test");
        frame.AddComponent(component, frame.Center.X, frame.Center.Y);
        _server = Server.Instance;
        _netID = id;
        frame.Display();
    }

    public override void Clear()
    { 
        var req = new Protocol()
            .SetValue("DISPLAY_ACTION", "CLEAR");
        _server.SendAndWaitForAnswer(this, req);
    }

    public override (int x, int y) GetSize()
    {
        throw new NotImplementedException();
    }

    public override void Prompt()
    {
        throw new NotImplementedException();
    }

    public override void SetCursorPosition(int x, int y)
    {
        throw new NotImplementedException();
    }

    public override void Write(string val)
    {
        var req = new Protocol()
            .SetValue("DISPLAY_ACTION", "DISPLAY")
            .SetValue("DISPLAY_CONTENT", val);
        _server.SendAndWaitForAnswer(this, req);
    }
}
