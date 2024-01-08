using System.Text.Json;

namespace Poker.Players.Net;

public class Protocol
{
    
    public IReadOnlyList<object> Keys => _values.Keys.ToList();

    private Dictionary<string, object> _values;
    


    public Protocol() {
        _values = new Dictionary<string, object>();
    }
    private Protocol(Dictionary<string, object> from) {
        _values = from;
    }

    public bool ContainsKey(string key) {
        return _values.ContainsKey(key);
    }

    public Protocol SetInt(string key, int value) {
        _values[key] = value;
        return this;
    }
    public Protocol SetString(string key, string value) {
        _values[key] = value;
        return this;
    }

    public int GetInt(string key) {
        return ((JsonElement)_values[key]).GetInt32();
    }
    public string GetString(string key) {
        return ((JsonElement)_values[key]).GetString() ?? "";
    }


    public string Serialize() {
        return JsonSerializer.Serialize(_values);
    }
    public static Protocol Parse(string value) {
        Dictionary<string, object> result = 
            JsonSerializer.Deserialize<Dictionary<string, object>>(value)
            ?? new Dictionary<string, object>();
        return new Protocol(result);
    }


}
