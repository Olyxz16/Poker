using GUISharp;

namespace Poker.Players;

public class RemoteConsolePlayer : ConsolePlayer, IDisplayable
{

    // Envoie les données au serveur, qu'il doit envoyer au client.

    public RemoteConsolePlayer(int balance, int x, int y) : base(balance)
    {
        frame = new Frame(this, x, y);
    }


    public override void Clear()
    {
        
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
        throw new NotImplementedException();
    }
}
