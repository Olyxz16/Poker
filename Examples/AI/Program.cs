using System.Linq;
using System.IO;
using Poker;
using Poker.Players.IA;
using Poker.Players.IA.Scenarios;
using Poker.Players.IA.Utils;

namespace AI;

public class Program {
    
    private const string MODEL_PATH = @"../../Models/";
    private const int DEFAULT_BAL = 200;

    public static void Main(string[] args) 
    {

        if(args.Contains("--train")) {
            Train();
        } else if(args.Contains("--play")) {
            var modelInd = Array.IndexOf(args, "--model");
            var modelName = args[modelInd+1];
            Play(modelName);
        } else {
            Console.WriteLine("Use --train or --play.");
        }

    }
    
    private static void Train() {
        var agent = new NeuralNetPlayer(DEFAULT_BAL);
        MoveTraining.TrainValidMovesFromRandomSamples(agent.Model, DEFAULT_BAL);
    }
    private static void Play(string modelName = "", bool save = true) {
        NeuralNetPlayer agent1;
        NeuralNetPlayer agent2;
        var modelPath = Path.Join(MODEL_PATH, modelName);
        modelPath = Path.GetFullPath(modelPath);
        if(modelName == "" || !File.Exists(modelPath)) {
            agent1 = new NeuralNetPlayer(DEFAULT_BAL);
            agent2 = new NeuralNetPlayer(DEFAULT_BAL);
        } else {
            agent1 = new NeuralNetPlayer(DEFAULT_BAL, modelPath);
            agent2 = new NeuralNetPlayer(DEFAULT_BAL, modelPath);
        }
        var game = new Game(new() { agent1, agent2 });
        var trainer = new TrainingUtils(game);
        game.Play();
        trainer.Train(agent1.Model, 3); 
        if(save) {
            if(modelName == "") {
                modelPath = Path.Join(modelPath, "model.h5");
                modelPath = Path.GetFullPath(modelPath);
            }
            agent1.Save(modelPath);
        }
    }

}
