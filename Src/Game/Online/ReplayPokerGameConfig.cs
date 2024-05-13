public struct ReplayPokerGameConfig {
    public string MAIL { get; init; } = "";
    public string PASS { get; init; } = "";
    public int TARGET_PRICE { get; init; } = 1;
    public int MAX_FREE_SEAT { get; init; } = 2;
    public bool DEBUG { get; init; } = false;
        
    public ReplayPokerGameConfig() {
        MAIL = "";
        PASS = "";
        TARGET_PRICE = 1;
        MAX_FREE_SEAT = 2;
        DEBUG = false;
    }

}
