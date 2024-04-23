using Poker.Players;

namespace Poker;

public struct GameEndState
{

    public readonly bool IsDraw;
    
    public List<Player> Players;
    public Player Winner;
    public List<Player> Winners;

    private GameEndState(List<Player> players, Player winner) {
        IsDraw = false;
        Players = players;
        Winner = winner;
        Winners = new List<Player>(0);
    }
    private GameEndState(List<Player> players, List<Player> winners) {
        IsDraw = true;
        Players = players;
        Winner = winners.First();
        Winners = winners;
    }


    public static GameEndState Win(List<Player> players, Player winner) {
        return new GameEndState(players, winner);
    }
    public static GameEndState Draw(List<Player> players, List<Player> winners) {
        return new GameEndState(players, winners);
    }

}
