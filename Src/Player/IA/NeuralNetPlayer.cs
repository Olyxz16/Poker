using Keras;
using Keras.Layers;
using Keras.Models;
using Numpy;
using Poker.Events;
using Poker.Players.IA.Utils;
using Python.Runtime;

namespace Poker.Players.IA;

public class NeuralNetPlayer : Player
{

    private static bool initialized = false;

    public Sequential Model { get; private set; }
    
    public delegate void OnPlayerMoveEventHandler(object sender, PlayerMoveEventArgs e);
    public event OnPlayerMoveEventHandler? OnPlayerMoveEvent;

    public NeuralNetPlayer(int balance, string weightFile = "") : base(balance) {
        if(!initialized) {
            np.arange(1);
            Python.Runtime.PythonEngine.BeginAllowThreads();
            initialized = true;
        }
        using(Py.GIL()) {
            Model = new Sequential();
            Init();
            if(File.Exists(weightFile)) {
                Model.LoadWeight(weightFile);
            }
        }
    }


    protected override Move ChoseMove(GameState state)
    {
        using(Py.GIL()) {
            var input = WeightUtils.InputFromState(state);
            input = input.reshape(1, 16);
            var output = Model.Predict(input, verbose:0);
            OnPlayerMoveEvent?.Invoke(this, new PlayerMoveEventArgs(this, input, output));
            var move = WeightUtils.MoveFromOutput(output, state);
            return move;
        }
    }


    private void Init() {
        Model.Add(new Dense(256, activation: "relu", input_shape: new Shape(16)));
        Model.Add(new Dense(256, activation: "relu"));
        Model.Add(new Dense(5, activation: "sigmoid"));
        
        Model.Compile(optimizer:"sgd", loss:"binary_crossentropy", metrics: new string[] { "accuracy" });
    }
    public void Save(string path) {
        using(Py.GIL()) {
            var p = Path.GetFullPath(path);
            if(!File.Exists(p)) {
                Directory.CreateDirectory(Path.GetDirectoryName(p) ?? "");
            }
            Model.SaveWeight(p);
        }
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
