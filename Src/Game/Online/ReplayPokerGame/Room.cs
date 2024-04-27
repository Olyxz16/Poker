namespace Poker.Online;

public struct Room {
    public string URL { get; init; }
    public bool Open { get; init; }
    public int Price { get; init; }
    public int MaxSeat { get; init; }
    public int BusySeat { get; init; }
    public Room(string url, bool open, int price, int maxSeat, int busySeat) {
        URL = url;
        Open = open;
        Price = price;
        MaxSeat = maxSeat;
        BusySeat = busySeat;
    }
}
