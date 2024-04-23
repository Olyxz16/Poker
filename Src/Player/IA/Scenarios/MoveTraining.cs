using Keras.Models;
using Poker.Cards;
using Poker.Players.IA.Utils;
using Numpy;

namespace Poker.Players.IA.Scenarios;


public static class MoveTraining {

    private static readonly int INPUT_SIZE = 16;
    private static readonly int TRAIN_BATCH_SIZE = 1000;


    public static void TrainValidMovesFromRandomSamples(Sequential model, int batch, bool verbose = true) {

        float accuracy = 0;
        while(accuracy < .99f) {
            
            var (states, inputs, outputs) = GenerateInputs(TRAIN_BATCH_SIZE);
            var ndinputs = np.array(inputs);
            var ndoutputs = np.array(outputs);
            model.Fit(ndinputs, ndoutputs, verbose?1:0);
            
            (states, inputs, outputs) = GenerateInputs(TRAIN_BATCH_SIZE);
            accuracy = Evaluate(model, states, inputs);
            Console.WriteLine($"{accuracy*100} / 100");

        }

    }

    private static (GameState[] states, NDarray[] inputs, NDarray[] outputs) GenerateInputs(int n) {
        var states = new GameState[n];
        var inputs = new NDarray[n];
        var outputs = new NDarray[n];
        for(int i = 0 ; i < n ; i++) {
            var state = GenerateRandomState();
            states[i] = state;
            NDarray ins = WeightUtils.InputFromState(state);
            inputs[i] = ins;
            NDarray outs = GenerateRandomOutputFromState(state);
            outputs[i] = outs;
        }
        return (states, inputs, outputs);
    }
    private static GameState GenerateRandomState() {
        var random = new Random();
        var deck = Deck.Standard52CardDeck();
        var round = random.Next(0, 4);
        var turn = random.Next(0,4);
        var bank = random.Next(0, 500);
        var player = new NeuralNetPlayer(100);
        player.DrawHand(deck.Draw(2));
        var bets = GenerateBets(turn);
        var flop = deck.Draw(5);
        var gamestate = new GameState(round, turn, bank, player, bets, flop);
        return gamestate;
    }
    private static Dictionary<string, int> GenerateBets(int turn) {
        var maxBet = 5*Game.SMALL_BLIND;
        var bets = new Dictionary<string, int>();
        for(int i = 0 ; i < turn ; i++) {
            if(i == 0) {
                bets.Add(i.ToString(), Game.SMALL_BLIND);
            } else if(i == 1) {
                bets.Add(i.ToString(),Game.BIG_BLIND);
            } else {
                bets.Add(i.ToString(), new Random().Next(bets.Values.Last(), maxBet));
            }
        }
        return bets;
    }
    private static NDarray GenerateRandomOutputFromState(GameState state) {
        Move move;
        float[] values;
        do {
            (move, values) = GetRandomMove(state);
        } while(!Move.IsValid(state, move, out string message));
        return np.array(values);
    }
    private static (Move, float[]) GetRandomMove(GameState state) {
        var random = new Random();
        var index = random.Next(0,5);
        var move = index switch {
            0 => Move.Fold(),
            1 => Move.Bet(0),
            2 => Move.Bet(state.Bets.Count > 0 ? state.Bets.Values.Max() : 0),
            3 => Move.Bet(0),
            _ => Move.Fold()
        };
        var outputs = index switch {
            0 => new float[] {1, 0, 0, 0, 0},
            1 => new float[] {0, 1, 0, 0, 0},
            2 => new float[] {0, 0, 1, 0, 0},
            3 => new float[] {0, 0, 0, (float)Sigmoid(state.Player.Balance), 0},
            _ => new float[] {0, 0, 0, 0, 1}
        };
        return (move, outputs);
    }
    private static float Evaluate(Sequential model, GameState[] states, NDarray[] inputs) {
        var valid = 0f;
        for(int i = 0 ; i < inputs.Length ; i++) { 
            var input = inputs[i].reshape(1, INPUT_SIZE);
            var output = model.Predict(input, verbose:0);
            var move = WeightUtils.MoveFromOutput(output, states[i]);
            valid += Move.IsValid(states[i], move, out _) ? 1: 0;
        }
        return valid / (float)inputs.Length;
    }
    private static double Sigmoid(double x) {
        return Math.Tanh(x/(2*4*Game.SMALL_BLIND));
    }

}
