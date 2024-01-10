namespace GUISharp.Components;

public class TextField : Component {

    public TextField(string text, int depth = 0) : base(text.Length, 1, depth) {
        for(int i = 0 ; i < text.Length ; i++) {
            _content[i,0] = text[i];
        }
    }

    public void SetText(string text) {
        int len = text.Length;
        _content = new char[len,1];
        for(int i = 0 ; i < len ; i++) {
            _content[i,0] = text[i];
        }
    }

}