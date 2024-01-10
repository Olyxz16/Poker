using Poker.Players;

namespace Poker;

public struct GameEndState
{

    public readonly bool IsDraw;

    public Player Winner;
    public List<Player> Winners;

    private GameEndState(Player winner) {
        IsDraw = false;
        Winner = winner;
        Winners = new List<Player>(0);
    }
    private GameEndState(List<Player> winners) {
        IsDraw = true;
        Winner = winners.First();
        Winners = winners;
    }


    public static GameEndState Win(Player winner) {
        return new GameEndState(winner);
    }
    public static GameEndState Draw(List<Player> winners) {
        return new GameEndState(winners);
    }

}
