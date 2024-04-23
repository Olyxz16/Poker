using Keras.Models;
using Numpy;
using Poker.Events;

namespace Poker.Players.IA.Utils;


public class TrainingUtils {

    private readonly List<(Player, NDarray, NDarray)> _entries = new();
    private readonly List<TrainingEntry> _trainingData = new();


    public TrainingUtils() {
        _entries = new();
        _trainingData = new();

        Game.GameStartEvent += AddPlayerEvents;
        Game.GameEndEvent += CompileResults;
    }

    public void AddPlayerEvents(object sender, GameStartEventArgs e) {
        var players = e.Players;
        foreach(var p in players) {
            if(p is NeuralNetPlayer neuralPlayer) {
                neuralPlayer.OnPlayerMoveEvent += AddEntry;
            }
        }
    }
    public void AddEntry(object sender, PlayerMoveEventArgs e) {
        _entries.Add((e.Player, e.Inputs, e.Outputs));
    }

    public void LoadData(string path) {
    }
    public void SaveData(string path) {
    }


    public void Train(Sequential model, int epochs) {
        for (int i = 0; i < epochs; i++)
        {
            // Iterate over the training data
            foreach (var data in _trainingData)
            {
                var state = data.inputs;
                var action = data.outputs;
                var reward = data.reward;

                var targetValue = reward > 0 ? 1 : -1;

                model.TrainOnBatch(state, action , new[] { targetValue });
            }
        }
    }


    private void CompileResults(object sender, GameEndEventArgs e) {
        var winner = e.State.Winner;
        foreach(var (player, inputs, outputs) in _entries) {
            var reward = player == winner ? 1d : 0d;
            var data = new TrainingEntry(inputs, outputs, reward);
            _trainingData.Add(data);
        }
    }

}
