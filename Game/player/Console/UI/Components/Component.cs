namespace GUISharp.Components;

public abstract class Component {

    protected Location _location;

    public int SizeX => _content.GetLength(0);
    public int SizeY => _content.GetLength(1);

    protected char[,] _content;
    public char[,] Content => (char[,])_content.Clone();

    public Component(int sizeX, int sizeY) {
        _content = new char[sizeX,sizeY];
        for(int x = 0 ; x < sizeX ; x++) {
            for(int y = 0 ; y < sizeY ; y++) {
                _content[x,y]=' ';
            }
        }
    }


    public void SetLocation(Location location) {
        _location = location;
    }
    public void SetCharAt(char c, int x, int y) {
        if(x < 0 || x >= SizeX || y < 0 || y >= SizeY) {
            throw new IndexOutOfRangeException("");
        }
        _content[x,y] = c;
    }
    public void SetRow(char[] chars, int row) {
        for(int x = 0 ; x < SizeX ; x++) {
            _content[x,row] = chars[x];
        }
    }
    public void SetRow(string str, int row) {
        SetRow(str.ToArray(), row);
    }
    public void SetColumn(char[] chars, int col) {
        for(int y = 0 ; y < SizeX ; y++) {
            _content[col,y] = chars[y];
        }
    }
    public void SetColumn(string str, int col) {
        SetColumn(str.ToArray(), col);
    }
    public void SetBorder(char c) {
        for(int i = 0 ; i < SizeX ; i++) {
            _content[i,0] = c;
            _content[i,SizeY-1] = c;
        }
        for(int i = 0 ; i < SizeY ; i++) {
            _content[0,i] = c;
            _content[SizeX-1,i] = c;
        }
    }

}
