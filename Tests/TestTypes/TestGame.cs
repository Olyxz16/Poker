using Poker.Players;
using Poker.Cards;
using Poker;

namespace Tests.TestTypes;

public class TestGame : Game
{

    private List<Card> _test_flop;

    public TestGame(List<Player> players, List<Card> flop) : base(players)
    {
        _test_flop = flop;
    }

    protected new void InitGame() {
        base.InitGame();
        _flop = _test_flop;
    }

}
