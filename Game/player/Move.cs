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


    public static bool IsValid(GameState state, Move chosenMove, out string errorMessage) {
        
        if(state.Round == 1) {
            if(state.Turn == 1) {
                if(chosenMove.MoveType == MoveType.FOLD) {
                    errorMessage = "You can't fold on first turn.";
                    return false;
                }
                if(chosenMove.BetValue != Game.SMALL_BLIND) {
                    errorMessage = $"You have to bet small blind : {Game.SMALL_BLIND}.";
                    return false;
                }
                errorMessage = "";
                return true;
            }
            if(state.Turn == 2) {
                if(chosenMove.BetValue != Game.BIG_BLIND) {
                    errorMessage = $"You have to bet big blind : {Game.BIG_BLIND}.";
                    return false;
                }
                errorMessage = "";
                return true;
            }
        }
        
        if(chosenMove.MoveType == MoveType.FOLD) {
            errorMessage = "";
            return true;
        }

        var currentBet = state.Bets.Count > 0 ? state.Bets.Values.Max() : 0;
        if(state.Bets.Count > 0 && chosenMove.BetValue < currentBet) {
            errorMessage = $"You have to bet over {currentBet}";
            return false;
        }
        var playerBet = chosenMove.BetValue;
        if(chosenMove.BetValue > playerBet) {
            errorMessage = "Insufficient balance.";
            return false;
        }
        errorMessage = "";
        return true;
    }

}

public enum MoveType {
    FOLD,
    BET
}
