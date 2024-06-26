using Poker.Cards;
using Poker.Events;
using Poker.Players;
using static Poker.IGameEvents;

namespace Poker;

public class Game : IGameEvents
{

    public static int DEFAULT_BALANCE = 100;
    public readonly static int SMALL_BLIND = 10;
    public static int BIG_BLIND => 2*SMALL_BLIND;

    
    protected List<Player> _players;
    public IReadOnlyList<Player> Players => _players;

    protected List<int> _bets;
    public IReadOnlyList<int> Bets => _bets;
    protected int MaxBetValue => _bets.Max();

    protected List<Card> _flop;
    public IReadOnlyList<Card> River => _flop;

    protected Deck _deck;
    protected int _bank;
    protected int _round;
    
    public event GameStartEventHandler? GameStartEvent;
    public event GameEndEventHandler? GameEndEvent;
    public event OnPlayerTurnEventHandler? OnPlayerTurnEvent;

    public Game(List<Player> players) {

        _players = players;
        _bets = new List<int>();
        _flop = new List<Card>();

        _deck = GetNewShuffledDeck();

        _bank = 0;
        _round = 0;

        GameStartEvent?.Invoke(this, new GameStartEventArgs(_players));

    }

    private static Deck GetNewShuffledDeck() {
        var deck = Deck.Standard52CardDeck();
        deck.Shuffle();
        return deck;
    }

    public Player Play() {
        Player winner = _players[0];
        while(Players.Count > 1) {
            winner = PlayGame() ?? winner;
            ConfirmEndGame(_players, GameEndState.Win(_players, winner));
            RemoveLosers();
        }
        return winner;
    }

    private Player? PlayGame() {
        InitGame();
        var remainingPlayers = new List<Player>(_players);
        for(_round = 1 ; _round <= 4 ; _round++) {
            _bets = new List<int>();
            PlayRound(remainingPlayers);
            SumPlayerBets();
        }
        var winner = ComputeWinner();
        return winner;
    }

    protected void InitGame() {
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
            UpdatePlayersUI(remainingPlayers, gameState);
            OnPlayerTurnEvent?.Invoke(this, new(gameState));
            var move = player.Play(gameState);
            if(move.MoveType == MoveType.FOLD) {
                remainingPlayers.Remove(player);
                i--;
                continue;
            }
            bool allowed = move.BetValue >= MaxBetValue || move.BetValue == player.Balance;
            if(move.MoveType == MoveType.BET && allowed) {
                _bets.Add(move.BetValue);
                player.Balance -= move.BetValue;
            } else {
                i--;
                continue;
            }
        }
    }

    private void SumPlayerBets() {
        foreach(var bet in _bets) {
            _bank += bet;
        }
    }

    public GameState GetGameState(int turn, Player player) {
        return new GameState(_round, turn, _bank, player, _bets, _flop);
    }
    private static void UpdatePlayersUI(List<Player> remainingPlayers, GameState gameState) {
        remainingPlayers.ForEach(p => p.UpdateUI(gameState));
    }
    // TODO Refactor this mess
    private void ConfirmEndGame(List<Player> remainingPlayers, GameEndState state) {
        Task.Run(async () => {
            var tasks = new List<Task<bool>>();
            foreach(var p in remainingPlayers) {
                var task = Task.Run(() => p.ConfirmGameEnd(state));
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }).GetAwaiter().GetResult();
        GameEndEvent?.Invoke(this, new GameEndEventArgs(state));
    }


    public void RemoveLosers() {
        for(int i = _players.Count-1 ; i >= 0; i--) {
            if(_players[i].Balance <= BIG_BLIND) {
                _players.RemoveAt(i);
            }
        }
    }

    private Player? ComputeWinner() {
        var winner = _players[0];
        var drawPlayers = new HashSet<Player>();
        for(int i = 0 ; i < _players.Count ; i++) {
            var newWinner = Combination.Compare(winner, _players[i], _flop);
            if(newWinner == null) {
                drawPlayers.Add(winner);
                drawPlayers.Add(_players[i]);
                continue;
            }
            if(newWinner != winner) {
                drawPlayers.Clear();
            }
            winner = newWinner;
        }
        if(drawPlayers.Count != 0) {
            ResolveDraw(drawPlayers);
            return null;
        }
        winner.Balance += _bank;
        return winner;
    }

    private void ResolveDraw(IEnumerable<Player> players) {
        int value = _bank / players.Count();
        foreach(var player in players) {
            player.Balance += value;
        }
    }


}
