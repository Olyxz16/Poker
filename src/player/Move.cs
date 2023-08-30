namespace Poker.Players;


public struct Move
{

    private readonly int _flag;

    public readonly MoveType MoveType => (MoveType)(_flag & 0b_0000_0001);
    public readonly int BetValue => _flag >> 1; 

    private Move(int flag) {
        _flag = flag;
    }

    public static Move Fold() {
        return new Move((int)MoveType.FOLD);
    }
    public static Move Bet(int value) {
        int flag = (value << 1) | (int)MoveType.BET;
        return new Move(flag);
    }
    
}

[Flags]
public enum MoveType {
    FOLD = 0b_0000_0000,
    BET = 0b_0000_0001
}
