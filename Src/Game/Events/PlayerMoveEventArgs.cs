using Numpy;
using Poker.Players;

namespace Poker.Events;

public class PlayerMoveEventArgs {
    public Player Player { get; }
    public NDarray Inputs { get; }
    public NDarray Outputs { get; }
    public PlayerMoveEventArgs(Player player, NDarray inputs, NDarray outputs) {
        Player = player;
        Inputs = inputs;
        Outputs = outputs;
    }
}
