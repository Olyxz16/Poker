using System.Text.Json;
using Microsoft.Playwright;

namespace Poker.Online.Utils;


public static class CardUtils { 
    
    private const string folder = @"./cards/";
    private const string jsonPath = @"./cards/raw.json";

    private static List<CardSVGPair>? pairs = null;

    public static async Task SaveSvgImageCardPairs(IPage page) {
        pairs ??= JsonSerializer.Deserialize<List<CardSVGPair>>(File.ReadAllText(jsonPath));
        var cardsLoc = page.Locator("div.Card__communityCards").Locator("div.Card");
        var cards = await cardsLoc.AllAsync();
        foreach(var card in cards) {
            var id = new Random().Next();
            var path = Path.Join(folder, $"{id}.png");
            try {
                await card.ScreenshotAsync(new() { Path = path });
            } catch {
               return; 
            } finally {
                var svg = await cardsLoc.Locator("path").AllAsync();
                foreach(var p in svg) {
                    string? attr = "";
                    await p.GetAttributeAsync("d").ContinueWith((res) => {
                        attr = res.Result;
                        if(attr == null) {
                            return;
                        }
                        var data = new CardSVGPair(attr, path);
                        pairs?.Add(data);
                    });
                }
            }
        }
        File.WriteAllText(jsonPath, JsonSerializer.Serialize(pairs));
    }

    public struct CardSVGPair {
        public string SVG { get; init; }
        public string ImagePath { get; init; }
        public CardSVGPair(string svg, string path) {
            SVG = svg;
            ImagePath = path;
        }
    }

}
