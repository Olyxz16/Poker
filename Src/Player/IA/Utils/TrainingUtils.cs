using Keras.Models;
using Poker.Events;

namespace Poker.Players.IA.Utils;


public class TrainingUtils {

    private readonly List<(GameState, Move)> _entries = new();
    private readonly List<TrainingEntry> _trainingData = new();


    public TrainingUtils() {
        _entries = new();
        _trainingData = new();

        Game.PlayerMoveEvent += AddEntry;
        Game.GameEndEvent += CompileResults;
    }


    public void AddEntry(object sender, PlayerMoveEventArgs e) {
        _entries.Add((e.State, e.Move));
    }

    public void LoadData(string path) {
    }
    public void SaveData(string path) {
    }


    public void Train(Model model, int epochs) {
        for (int i = 0; i < epochs; i++)
        {
            // Iterate over the training data
            foreach (var data in _trainingData)
            {
                var state = data.inputs;
                var action = data.outputs;
                var reward = data.reward;

                var targetValue = reward > 0 ? 1 : -1;

                model.TrainOnBatch(new[] { state }, new[] { action }, new[] { targetValue });
            }
        }
    }


    private void CompileResults(object sender, GameEndEventArgs e) {
        var winner = e.State.Winner;
        foreach(var (state, move) in _entries) {
            var data = Compile(state, move, winner);
            _trainingData.Add(data);
        }
    }
    private TrainingEntry Compile(GameState state, Move move, Player winner) {
        var inputs = CompileInputs(state);
        var outputs = CompileOutputs(move, state.Player.Balance);
        var reward = state.Player == winner ? 1d : 0d;
        return new TrainingEntry(inputs, outputs, reward);
    }

    private double[] CompileInputs(GameState state) {
        var player = state.Player;
        var balance = player.Balance;
        var flop = state.Flop;
        var maxPot = state.Bets.Max().Value;

        var inputs = new List<double>();
        var playerCard1 = WeightUtils.WeightsFromCard(player.Hand[0]);
        var playerCard2 = WeightUtils.WeightsFromCard(player.Hand[1]);
        var flopCard1 = WeightUtils.WeightsFromCard(flop[0]);
        var flopCard2 = WeightUtils.WeightsFromCard(flop[1]);
        var flopCard3 = WeightUtils.WeightsFromCard(flop[2]);
        var flopCard4 = WeightUtils.WeightsFromCard(flop[3]);
        var flopCard5 = WeightUtils.WeightsFromCard(flop[4]);
        var maxPotVal = Math.Min(maxPot / player.Balance, 1);
        var balanceVal = WeightUtils.WeightFromBalance(balance);

        inputs.AddRange(playerCard1);
        inputs.AddRange(playerCard2);
        inputs.AddRange(flopCard1);
        inputs.AddRange(flopCard2);
        inputs.AddRange(flopCard3);
        inputs.AddRange(flopCard4);
        inputs.AddRange(flopCard5);
        inputs.Add(maxPotVal);
        inputs.Add(balanceVal);

        return inputs.ToArray();
    }
    // WARNING : NOT THE ACTUAL DATA, JUST A RECONSTRUCTION. WOULD NEED A WAY TO PULL DATA FROM MODEL WHEN CONSTRUCTION MOVE.
    private double[] CompileOutputs(Move move, int balance) {
        if(move.MoveType == MoveType.FOLD) {
            return new[] {1d, 0d, 0d, 0d};
        } else if(move.BetValue > 0) {
            var value = WeightUtils.WeightFromBalance(balance);
            return new[] {0d, 0d, 1d, value};
        }
        else {
            return new[] {0d, 1d, 0d, 0d};
        }
    }

}