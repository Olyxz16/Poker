using Poker.Players.IA;
using Poker.Online;

public class Program {
    
    private string ModelPath = @"../../Models/model.h5";
    
    public static void Main(String[] args) {
        var config = ParseArgs(args);
        var player = LoadPlayer();
        var game = new ReplayPokerGame(player, config);
        game.Play();
    }

    private static NeuralNetPlayer LoadPlayer() {
        var player = new NeuralNetPlayer(100, ModelPath);
        return player;
    }

    private static ReplayPokerGameConfig ParseArgs(String[] args) {
        var modelIndex = args.FindIndex("--model");
        ModelPath = args[args[modelIndex+1]];
        
        var config = new ReplayPokerGameConfig() {
            MAIL = args[args.FindIndex("--mail")+1] ?? "",
            PASS = args[args.FindIndex("--pass")+1] ?? "",
        };
        return config;
    }

}
