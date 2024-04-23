using Numpy;
using Poker.Cards;

namespace Poker.Players.IA.Utils;


public static class WeightUtils {
    

    public static NDarray InputFromState(GameState state) {
        var player = state.Player;
        var balance = player.Balance;
        var flop = state.Flop;
        var maxPot = state.Bets.Count > 0 ? state.Bets.Values.Max() : 0;

        var inputs = new List<double>();
        var playerCard1 = WeightsFromCard(player.Hand[0]);
        var playerCard2 = WeightsFromCard(player.Hand[1]);
        var flopCard1 = WeightsFromCard(flop.ElementAtOrDefault(0));
        var flopCard2 = WeightsFromCard(flop.ElementAtOrDefault(1));
        var flopCard3 = WeightsFromCard(flop.ElementAtOrDefault(2));
        var flopCard4 = WeightsFromCard(flop.ElementAtOrDefault(3));
        var flopCard5 = WeightsFromCard(flop.ElementAtOrDefault(4));
        // To avoid division by zero but janky
        var maxPotVal = player.Balance > 0 ? Math.Min(maxPot / player.Balance, 1) : 1;
        var balanceVal = WeightFromBalance(balance);

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


    // Move
    public static Move MoveFromOutput(NDarray output, GameState state) {
        var ind = MaxInd(output);
        var maxBet = state.Bets.Count() == 0 ? 0: state.Bets.Values.Max();
        var move = ind switch {
            0 => Move.Fold(),
            1 => Move.Bet(0),
            2 => Move.Bet(maxBet),
            // not sure
            3 => Move.Bet(BalanceFromWeight(output.item<double>(4))),
            _ => Move.Fold()
        };
        // failsafe
        var bal = state.Player.Balance;
        if(state.Turn == 1) {
            return Move.Bet(Math.Min(Game.SMALL_BLIND, bal));
        }
        if(state.Turn == 2) {
            return Move.Bet(Math.Min(Game.BIG_BLIND, bal));
        }
        if(move.BetValue > bal) {
            return Move.Bet(bal);
        }
        return move;
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
