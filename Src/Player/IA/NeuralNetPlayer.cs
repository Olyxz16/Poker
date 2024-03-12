using Keras;
using Keras.Layers;
using Keras.Models;
using Numpy;
using Poker.Players.IA.Utils;

namespace Poker.Players.IA;

public class NeuralNetPlayer : Player
{

    public Sequential Model { get; private set; }


    public NeuralNetPlayer(int balance) : base(balance)
    {
        Model = new Sequential();
        Init();
    }
    public NeuralNetPlayer(int balance, string weightFile) : base(balance) {
        // Problème d'ordre d'exécution ?
        Model = Load(weightFile);
        Init();
    }


    protected override Move ChoseMove(GameState state)
    {
        var input = InputFromState(state);
        var output = Model.Predict(input);
        var move = WeightUtils.MoveFromOutput(output, state);
        return move;
    }
    
    private static NDarray InputFromState(GameState state) {
        var player = state.Player;
        var balance = player.Balance;
        var flop = state.Flop;
        var maxPot = state.Bets.Values.Max();

        var inputs = new List<double>();
        var playerCard1 = WeightUtils.WeightsFromCard(player.Hand[0]);
        var playerCard2 = WeightUtils.WeightsFromCard(player.Hand[1]);
        var flopCard1 = WeightUtils.WeightsFromCard(flop.ElementAtOrDefault(0));
        var flopCard2 = WeightUtils.WeightsFromCard(flop.ElementAtOrDefault(1));
        var flopCard3 = WeightUtils.WeightsFromCard(flop.ElementAtOrDefault(2));
        var flopCard4 = WeightUtils.WeightsFromCard(flop.ElementAtOrDefault(3));
        var flopCard5 = WeightUtils.WeightsFromCard(flop.ElementAtOrDefault(4));
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
        
        return np.array(inputs.ToArray());
    }


    private void Init() {
        Model.Add(new Dense(32, activation: "relu", input_shape: new Shape(16)));
        Model.Add(new Dense(64, activation: "relu"));
        Model.Add(new Dense(5, activation: "sigmoid"));
        
        Model.Compile(optimizer:"sgd", loss:"binary_crossentropy", metrics: new string[] { "accuracy" });
    }
    public void Save(string fileName) {
        Model.SaveWeight(fileName);
    }
    public static Sequential Load(string fileName) {
        var model = new Sequential();
        model.LoadWeight(fileName);
        return model;
    }

    

    public override Task<bool> ConfirmGameEnd(GameEndState state)
    {
        return Task.Run(() => true);
    }
    protected override void DisplayErrorMessage(string message)
    {}
    protected override void DisplayGameState(GameState state)
    {}

}