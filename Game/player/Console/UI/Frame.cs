using System.Text;
using GUISharp.Components;
using GUISharp.Collections;
using Poker.Players;

namespace GUISharp;

public class Frame
{

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public (int X, int Y) Center => (SizeX/2, SizeY/2);

    private readonly IDisplayable? _target;
    private readonly DepthLinkedList _components;

    private Canvas _canvas;


    public Frame(IDisplayable? target, int sizeX, int sizeY) {
        SizeX = sizeX;
        SizeY = sizeY;
        _target = target;
        _components = new DepthLinkedList();
        _canvas = new Canvas(SizeX, SizeY, 99);
    }
    public static Frame Empty() {
        return new Frame(null, 0, 0);
    }

    public void SetCursorPosition(int x, int y) {
        if(_target == null) {
            throw new NullReferenceException("Target cannot be null.");
        }
        _target.SetCursorPosition(x,y);
    }
    public (int x, int y) GetCursorPosition() {
        if(_target == null) {
            throw new NullReferenceException("Target cannot be null.");
        }
        return _target.GetCursorPosition();
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

    public void ClearComponents() {
        _components.Clear();
    }

    public void Display() {
        if(_target == null) {
            throw new NullReferenceException("Target cannot be null.");
        }
        _target.Clear();
        var str = BuildComponents();
        _target.Write(str);
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

}
