using Poker.Players;
using Poker.Players.IA;
using Poker.Online;

namespace Online;

class Program {
    
    public static void Main(String[] args) {
    
        //var agent = new NeuralNetPlayer(100);
        //var p = new LocalConsolePlayer(100);
        
        Task.Run(async () => {
                var game = new ReplayPokerGame(null);
                await game.Play(); 

                }).Wait();

    }

}
