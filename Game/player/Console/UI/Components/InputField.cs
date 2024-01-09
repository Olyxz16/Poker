using GUISharp;
using GUISharp.Components;

namespace Poker.Players.UI;

public class InputField : Component
{

    private Frame _frame;
    private string _prompt;

    public InputField(Frame frame, string prompt) : base(prompt.Length, 1) {
        _frame = frame;
        _prompt = prompt;
        for(int i = 0 ; i < prompt.Length ; i++) {
            _content[i,0] = prompt[i];
        }
    }

    
    public string Prompt() {
        (int PreviousCursorX, int PreviousCursorY) = _frame.GetCursorPosition();
        _frame.SetCursorPosition(PosX + _prompt.Length, PosY);
        var result = Console.ReadLine() ?? "";
        _frame.SetCursorPosition(PreviousCursorX, PreviousCursorY);
        return result;
    }
}
