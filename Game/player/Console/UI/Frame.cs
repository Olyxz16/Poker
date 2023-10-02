using System.Text;
using GUISharp.Components;

namespace GUISharp;

public class Frame
{

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public (int X, int Y) Center => (SizeX/2, SizeY/2);

    private readonly char[,] characters;


    public Frame(int sizeX, int sizeY) {
        SizeX = sizeX;
        SizeY = sizeY;
        characters = new char[SizeX,SizeY];
        for(int x = 0 ; x < SizeX ; x++) {
            for(int y = 0 ; y < SizeY ; y++) {
                characters[x,y] = ' ';
            }
        }
    }

    public void SetCharAt(char c, int x, int y) {
        if(x < 0 || x >= SizeX || y < 0 || y >= SizeY) {
            throw new IndexOutOfRangeException("");
        }
        characters[x,y] = c;
    }
    public void SetRow(char[] chars, int row) {
        for(int x = 0 ; x < SizeX ; x++) {
            characters[x,row] = chars[x];
        }
    }
    public void SetRow(string str, int row) {
        SetRow(str.ToArray(), row);
    }
    public void SetColumn(char[] chars, int col) {
        for(int y = 0 ; y < SizeX ; y++) {
            characters[col,y] = chars[y];
        }
    }
    public void SetColumn(string str, int col) {
        SetColumn(str.ToArray(), col);
    }
    public void SetBorder(char c) {
        for(int i = 0 ; i < SizeX ; i++) {
            characters[i,0] = c;
            characters[i,SizeY-1] = c;
        }
        for(int i = 0 ; i < SizeY ; i++) {
            characters[0,i] = c;
            characters[SizeX-1,i] = c;
        }
    }


    public void SetCursorPosition(int x, int y) {
        if(x < 0 || x >= SizeX || y < 0 || y >= SizeY) {
            throw new IndexOutOfRangeException("");
        }
        Console.SetCursorPosition(x,y);
    }
    
    public void AddComponent(Component component, int xOff, int yOff, bool refresh = false) {
        if(component.SizeX+xOff >= SizeX || component.SizeY+yOff >= SizeY) {
            throw new ArgumentOutOfRangeException();
        }
        for(int x = 0 ; x < component.SizeX ; x++) {
            for(int y = 0 ; y < component.SizeY ; y++) {
                int PosX = x+xOff;
                int PosY = y+yOff;
                characters[PosX, PosY] = component.Content[x,y];
            }
        }
        if(refresh) {
            Display();
        }
    }

    public void Display() {
        Console.Clear();
        var stringBuilder = new StringBuilder();
        for(int y = 0 ; y < SizeY-1 ; y++) {
            for(int x = 0 ; x < SizeX ; x++) {
                stringBuilder.Append(characters[x,y]);
            }
            stringBuilder.Append('\n');
        }
        for(int x = 0 ; x < SizeX ; x++) {
            stringBuilder.Append(characters[x,SizeY-1]);
        }
        Console.Write(stringBuilder.ToString());
    }

    public static Frame GetCurrentFrame() {
        int sizeX = Console.WindowWidth;
        int sizeY = Console.WindowHeight;
        return new Frame(sizeX, sizeY);
    }

}
