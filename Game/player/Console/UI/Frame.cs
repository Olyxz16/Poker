using System.Text;
using GUISharp.Components;
using GUISharp.Collections;

namespace GUISharp;

public class Frame
{

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public (int X, int Y) Center => (SizeX/2, SizeY/2);

    private Canvas _canvas;

    private readonly DepthLinkedList _components;

    public delegate void ClearDelegate();
    public delegate void WriteDelegate(string message);
    private ClearDelegate? clear;
    private WriteDelegate? write;

    public Frame(int sizeX, int sizeY) {
        SizeX = sizeX;
        SizeY = sizeY;
        _canvas = new Canvas(SizeX, SizeY, 99);
        _components = new DepthLinkedList();
    }

    public void SetDisplayDelegates(ClearDelegate clearDele, WriteDelegate writeDele) {
        clear = clearDele;
        write = writeDele;
    }

    public void SetCursorPosition(int x, int y) {
        if(x < 0 || x >= SizeX || y < 0 || y >= SizeY) {
            throw new IndexOutOfRangeException("");
        }
        Console.SetCursorPosition(x,y);
    }

    public void SetCharAt(char c, int x, int y) {
        _canvas.SetCharAt(c,x,y);
    }
    public void SetRow(char[] chars, int row) {
        _canvas.SetRow(chars, row);
    }
    public void SetRow(string str, int row) {
        _canvas.SetRow(str.ToArray(), row);
    }
    public void SetColumn(char[] chars, int col) {
        _canvas.SetColumn(chars, col);
    }
    public void SetColumn(string str, int col) {
        _canvas.SetColumn(str.ToArray(), col);
    }
    public void SetBorder(char c) {
        _canvas.SetBorder(c);
    }

    public void AddComponent(Component component, int xOff, int yOff, bool refresh = false) {
        component.SetPosition(xOff, yOff);
        _components.Add(component);
        if(refresh) {
            Display();
        }
    }

    public void Display() {
        if(clear == null || write == null) {
            throw new NullReferenceException("Display functions cannot be null.");
        }
        clear();
        var str = BuildComponents();
        write(str);
    }
    private string BuildComponents() {
        var characters = new char[SizeX,SizeY];
        for(int x = 0 ; x < SizeX ; x++) {
            for(int y = 0 ; y < SizeY ; y++) {
                characters[x,y] = ' ';
            }
        }
        foreach(var component in _components) {
            characters = Build(component, characters);
        }
        return ToString(characters);
    }
    private char[,] Build(Component component, char[,] characters) {
        int sizeX = component.SizeX;
        int sizeY = component.SizeY;
        for(int x = 0 ; x < sizeX ; x++) {
            int posX = component.PosX + x;
            if(posX >= SizeX) {
                continue;
            }
            for(int y = 0 ; y < sizeY ; y++) {
                int posY = component.PosY + y;
                if(posY >= SizeY) {
                    continue;
                }
                characters[posX,posY] = component.Content[x,y];
            }
        }
        return characters;
    }
    private string ToString(char[,] characters) {
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
        return stringBuilder.ToString();
    }
    

    public static Frame GetCurrentFrame() {
        int sizeX = Console.WindowWidth;
        int sizeY = Console.WindowHeight;
        return new Frame(sizeX, sizeY);
    }

}
