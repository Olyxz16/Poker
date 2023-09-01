using Poker.Cards;
using Poker.Players;

namespace Poker;

public class Game
{

    public readonly static int SMALL_BLIND = 10;
    public static int BIG_BLIND => 2*SMALL_BLIND;

    
    private List<Player> _players;
    public IReadOnlyList<Player> Players { get => _players; }

    private List<int> _bets;
    public IReadOnlyList<int> Bets { get => _bets; }

    private List<Card> _flop;
    public IReadOnlyList<Card> River { get => _flop; }

    private Deck _deck;
    private int _bank;


    public Game(List<Player> players) {

        _players = players;
        _bets = new List<int>();
        _flop = new List<Card>();

        _deck = GetNewShuffledDeck();

        _bank = 0;

    }

    private static Deck GetNewShuffledDeck() {
        var deck = Deck.Standard52CardDeck();
        deck.Shuffle();
        return deck;
    }

    public void Run() {
        while(Players.Count > 1) {
            var winner = PlayGame();
            RemoveLosers();
        }
    }

    private Player PlayGame() {
        InitGame();
        var remainingPlayers = new List<Player>(_players);
        for(int round = 1 ; round < 3 ; round++) {
            PlayRound(remainingPlayers);
        }
        var winner = ComputeWinner();
        winner.Balance += _bank;
        return winner;
    }

    private void InitGame() {
        _bank = 0;
        _deck = GetNewShuffledDeck();
        _flop = _deck.Draw(5);
        foreach(var player in _players) {
            player.DrawHand(_deck.Draw(2));
        }
    }

    private void PlayRound(List<Player> remainingPlayers) {
        for(int i = 0 ; i < remainingPlayers.Count ; i++) {
            var player = remainingPlayers[i];
            var turn = i+1;
            var gameState = GetGameState(turn, player);
            var move = player.Play(gameState);
            if(move.MoveType == MoveType.FOLD) {
                remainingPlayers.Remove(player);
                i--;
            } else if(move.MoveType == MoveType.BET) {
                _bank += move.BetValue;
            }
        }
    }

    public GameState GetGameState(int turn, Player player) {
        return new GameState(turn, player, _bets, _flop);
    }


    public void RemoveLosers() {
        for(int i = _players.Count-1 ; i >= 0; i--) {
            if(_players[i].Balance <= 0) {
                _players.RemoveAt(i);
            }
        }
    }

    private Player ComputeWinner() {
        var winner = _players[0];
        for(int i = 0 ; i < _players.Count ; i++) {
            winner = Combination.Compare(winner, _players[i], _flop);
        }
        return winner; 
    }


}
