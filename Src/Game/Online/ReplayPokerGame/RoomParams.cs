public struct RoomParams {
    public int? Price { get; init; }
    public int? MaxSeat { get; init; }
    public int? BusySeat { get; init; }
    public int? AvailableSeat { get; init; }
    public RoomParams(int? price = null, int? maxSeat = null, int? busySeat = null, int? availableSeat = null) {
        Price = price;
        MaxSeat = maxSeat;
        BusySeat = busySeat;
        AvailableSeat = availableSeat;
    }
}
