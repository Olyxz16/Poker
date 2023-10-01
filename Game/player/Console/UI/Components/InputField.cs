using GUISharp.Components;

namespace Poker.Players.UI;

public class InputField : Component
{

    private string _prompt;

    public InputField(string prompt) : base(prompt.Length, 1) {
        _prompt = prompt;
        for(int i = 0 ; i < prompt.Length ; i++) {
            _content[i,0] = prompt[i];
        }
    }

    public string Prompt() {
        (int PreviousCursorX, int PreviousCursorY) = Console.GetCursorPosition();
        Console.SetCursorPosition(_location.MinX + _prompt.Length, _location.MinY);
        var result = Console.ReadLine() ?? "";
        Console.SetCursorPosition(PreviousCursorX, PreviousCursorY);
        return result;
    }
}
