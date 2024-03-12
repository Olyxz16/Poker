using Numpy;
using Poker.Cards;

namespace Poker.Players.IA.Utils;


public static class WeightUtils {
    
    // Move
    public static Move MoveFromOutput(NDarray output, GameState state) {
        var ind = MaxInd(output);
        var maxBet = state.Bets.Values.Max();
        return ind switch {
            0 => Move.Fold(),
            1 => Move.Bet(0),
            2 => Move.Bet(maxBet),
            // not sure
            3 => Move.Bet(BalanceFromWeight(output.item<double>(4))),
            _ => Move.Fold()
        };
    }

    private static int MaxInd(NDarray array) {
        int len = array.size;
        double max = 0;
        int maxInd = 0;
        for(int i = 0 ; i < len ; i++) {
            var val = array.item<double>(i);
            if(val > max) {
                max = val;
                maxInd = i;
            }
        }
        return maxInd;
    }


    // Card
    public static double[] WeightsFromCard(Card card) {
        var color = card.Color switch {
            CardColor.CLUB => 0.25,
            CardColor.DIAMOND => 0.5,
            CardColor.SPADE => 0.75,
            CardColor.HEART => 1,
            _ => 0
        };
        var rank = card.Rank / 13d;
        return new double[] { color, rank };
    }


    // Balance
    public static double WeightFromBalance(int balance) {
        return Sigmoid(balance);
    }
    public static int BalanceFromWeight(double weight) {
        return (int)Math.Round(InvSigmoid(weight));
    }

    private static double InvSigmoid(double y) {
        return 4*Game.SMALL_BLIND*Math.Log(y+1) - 4*Game.SMALL_BLIND*Math.Log(1-y);
    }
    private static double Sigmoid(double x) {
        return Math.Tanh(x/(2*4*Game.SMALL_BLIND));
    }

}