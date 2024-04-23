using Numpy;

namespace Poker.Players.IA.Utils;

public struct TrainingEntry {
    
    public readonly NDarray inputs;
    public readonly NDarray outputs;
    public readonly double reward;

    public TrainingEntry(NDarray inputs, NDarray outputs, double reward) {
        this.inputs = inputs;
        this.outputs = outputs;
        this.reward = reward;
    }

}
