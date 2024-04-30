using System.Text.Json;
using Microsoft.Playwright;
using Poker.Cards;

namespace Poker.Online;


public static class CardUtils {

    public static async Task WaitForNewTurn(IPage page) {
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card");
        while(await cardsLoc.CountAsync() > 0);
        return;
    }

    public static async Task<List<Card>> GetFlop(IPage page) {
        var cardsLoc = page.Locator("div.Card__communityCards").Locator("div.Card");
        return null; 
    }



    private static async Task SaveSvgImageCardPairs(IPage page) {
        var folder = @"./cards/";
        var jsonPath = Path.Join(folder, "cards.json");
        var values = JsonSerializer.Deserialize<List<CardSVGPair>>(File.ReadAllText(jsonPath));
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card");
        
        Console.WriteLine("Running.");
        var running = true;
        new Thread(() => {
            Console.ReadKey();
            running = false;
                }).Start();
        while(running)
        {
            while(await cardsLoc.CountAsync() < 5);
                       
            while(await cardsLoc.CountAsync() > 0);
        }
        Console.WriteLine("Saving.");
        
        File.WriteAllText(jsonPath, JsonSerializer.Serialize(values));
    }
    private struct CardSVGPair {
        public string SVG { get; init; }
        public string ImagePath { get; init; }
        public CardSVGPair(string svg, string path) {
            SVG = svg;
            ImagePath = path;
        }
    }


}
