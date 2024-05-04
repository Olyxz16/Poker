﻿using Poker.Players;
using Poker.Players.IA;
using Poker.Online;
using Poker.Online.Utils;

namespace Online;

class Program {
    
    public static void Main(String[] args) {
    
        if(args.Contains("--analyze")) {
            AnalyzeCards();
        } else {
            Run();
        } 

    }

    private static void Run() {
        Task.Run(async () => {
                var game = new ReplayPokerGame(null);
                await game.Play(); 

                }).Wait();
    }

    private static void AnalyzeCards() {
        var analyzer = new CardAnalyzer();
        analyzer.Run();
    }

}
