using Poker;
using Poker.Players.IA;

namespace AI;

public class Program {

    public static void Main(string[] args) 
    {

        var agent1 = new NeuralNetPlayer(200);
        var agent2 = new NeuralNetPlayer(200);
        var game = new Game(new() { agent1, agent2 });
        game.Play();

    }

}