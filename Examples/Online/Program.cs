using Poker.Players;
using Poker.Players.IA;
using Poker.Online;
using Poker.Online.Utils;

namespace Online;

class Program {
   
    private const string MODEL_PATH = @"../../Models/model.h5";

    public static void Main(String[] args) {
    
        if(args.Contains("--analyze")) {
            AnalyzeCards();
        } else {
            Run();
        } 

    }

    private static void Run() {
        NeuralNetPlayer? player = new NeuralNetPlayer(100, MODEL_PATH);
        var config = new ReplayPokerGameConfig() {
            DEBUG = true
        };
        Task.Run(async () => {
                var game = new ReplayPokerGame(player, config);
                await game.Play(); 
                }).Wait();
    }

    private static void AnalyzeCards() {
        var analyzer = new CardAnalyzer();
        analyzer.Run();
    }

}
