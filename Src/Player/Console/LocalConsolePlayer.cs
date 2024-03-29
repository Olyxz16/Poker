using GUISharp;

namespace Poker.Players;

public class LocalConsolePlayer : ConsolePlayer, IDisplayable
{
    public LocalConsolePlayer(int balance) : base(balance)
    {
        frame = new Frame(this, Console.WindowWidth, Console.WindowHeight);
        DisplayWaitingScreen();
    }

    public override void Write(string val)
    {
        Console.Write(val);
    }

    public override void Clear()
    {
        Console.Clear();
    }

    public override string Prompt()
    {
        return Console.ReadLine() ?? "";
    }

    public override void SetCursorPosition(int x, int y) 
    {
        Console.SetCursorPosition(x,y);
    }

    public override (int x, int y) GetSize() {
        int x = Console.WindowWidth;
        int y = Console.WindowHeight;
        return (x,y);
    }

    public override (int x, int y) GetCursorPosition()
    {
        return Console.GetCursorPosition();
    }



}
