
namespace Poker.Players;

public interface IDisplayable
{
    
    public (int x, int y) GetSize();

    public void Write(string val);
    public void Clear();
    public void Prompt();
    public void SetCursorPosition(int x, int y);

}
