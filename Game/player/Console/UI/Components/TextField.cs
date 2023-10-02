

namespace GUISharp.Components;

public class TextField : Component {

    public TextField(string text) : base(text.Length, 1) {
        for(int i = 0 ; i < text.Length ; i++) {
            _content[i,0] = text[i];
        }
    }

}