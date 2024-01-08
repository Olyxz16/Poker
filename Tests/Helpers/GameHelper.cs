using Poker.Players;
using Poker.Cards;
using Poker;

namespace Tests.Helpers;

public class GameHelper : Game
{

    private List<Card> _test_flop;

    public GameHelper(List<Player> players, List<Card> flop) : base(players)
    {
        _test_flop = flop;
    }

    protected new void InitGame() {
        base.InitGame();
        _flop = _test_flop;
    }

}
