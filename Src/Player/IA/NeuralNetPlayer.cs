using Keras;
using Keras.Layers;
using Keras.Models;
using Poker.Events;
using Poker.Players.IA.Utils;

namespace Poker.Players.IA;

public class NeuralNetPlayer : Player
{

    public Sequential Model { get; private set; }
    
    public delegate void OnPlayerMoveEventHandler(object sender, PlayerMoveEventArgs e);
    public event OnPlayerMoveEventHandler? OnPlayerMoveEvent;

    public NeuralNetPlayer(int balance) : base(balance)
    {
        Model = new Sequential();
        Init();
    }
    public NeuralNetPlayer(int balance, string weightFile) : base(balance) {
        // Problème d'ordre d'exécution ?
        //Model = Load(weightFile);
        Model = new Sequential();
        Init();
        Model.LoadWeight(weightFile);
    }


    protected override Move ChoseMove(GameState state)
    {
        var input = WeightUtils.InputFromState(state);
        input = input.reshape(1, 16);
        var output = Model.Predict(input, verbose:0);
        OnPlayerMoveEvent?.Invoke(this, new PlayerMoveEventArgs(this, input, output));
        var move = WeightUtils.MoveFromOutput(output, state);
        return move;
    }


    private void Init() {
        Model.Add(new Dense(256, activation: "relu", input_shape: new Shape(16)));
        Model.Add(new Dense(256, activation: "relu"));
        Model.Add(new Dense(5, activation: "sigmoid"));
        
        Model.Compile(optimizer:"sgd", loss:"binary_crossentropy", metrics: new string[] { "accuracy" });
    }
    public void Save(string path) {
        var p = Path.GetFullPath(path);
        if(!File.Exists(p)) {
            Directory.CreateDirectory(Path.GetDirectoryName(p) ?? "");
            File.Create(p);
        }
        Model.SaveWeight(p);
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
    {
        Console.WriteLine(message);
    }
    protected override void DisplayGameState(GameState state)
    {}

}
