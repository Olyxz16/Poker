namespace Poker.Players.IA.Utils;

public struct TrainingEntry {
    
    public readonly double[] inputs;
    public readonly double[] outputs;
    public readonly double reward;

    public TrainingEntry(double[] inputs, double[] outputs, double reward) {
        this.inputs = inputs;
        this.outputs = outputs;
        this.reward = reward;
    }

}