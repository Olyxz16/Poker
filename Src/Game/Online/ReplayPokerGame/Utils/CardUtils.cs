using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Poker.Cards;

namespace Poker.Online.Utils;


public static class CardUtils { 
    
    private const string folder = @"./cards/";
    private const string jsonPath = @"./cards/raw.json";
    private static Dictionary<string, int> SVGToRank => CardSVGPair.Pairs;

    public static async Task<GameState> GetGameStateFromBoard(IPage page) {
        var turn = await GetTurn(page);
        var bank = await GetBank(page);
        var hand = await GetHand(page);
        var flop = await GetFlop(page);
        var round = flop.Count switch {
            0 => 1,
            3 => 2,
            4 => 3,
            5 => 4,
            _ => -1
        };

        var state = new GameState(round, turn, bank, null, new(), flop);
        return state;
    }
    private static async Task<int> GetTurn(IPage page) {
        return -1;
    }
    // TO BE REMADE
    private static async Task<int> GetBank(IPage page) {
        var bankLoc = page.Locator(".Pot__value").First;
        var bankString = await bankLoc.InnerTextAsync();
        var bank = Int32.Parse(bankString);
        return bank;
    }
    private static async Task<List<Card>> GetHand(IPage page) {
        return new List<Card>();
    }
    private static async Task<List<Card>> GetFlop(IPage page) {
        var result = new List<Card>();
        var cardsLoc = await page.Locator("div.Cards__communityCards").Locator("div.Card").AllAsync();
        foreach(var loc in cardsLoc) {
            var card = await GetCard(loc);
            Console.Write(card.Rank + " " + card.Color + " | ");
            result.Add(card);
        }
        Console.WriteLine();
        return result;
    }
    private static async Task<Card> GetCard(ILocator loc) {
        var svgLoc = loc.Locator("svg.Card__side.Card__side--front"); 
        var pipsLoc = svgLoc.Locator("g.Card__pips");
        var colorString = (await pipsLoc.GetAttributeAsync("class")??"").Split(" ")[1].Substring(12);
        var color = colorString switch {
            "heart" => CardColor.HEART,
            "diamond" => CardColor.DIAMOND,
            "spade" => CardColor.SPADE,
            "club" => CardColor.CLUB,
            _ => CardColor.CLUB
        };
        var cardFaceLoc = svgLoc.Locator("g.Card__face");
        if(await cardFaceLoc.CountAsync() > 0) {
            var attr = (await cardFaceLoc.GetAttributeAsync("class")??"");
            var rankString = attr[22];
            var rank = rankString switch {
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                _ => -1
            };
            return new Card(rank, color);
        }
        var paths = await loc.Locator("path").AllAsync();
        foreach(var path in paths) {
            var d = await path.GetAttributeAsync("d");
            if(d == null) {
                continue;
            }
            d = CleanSVG(d);
            foreach(var key in SVGToRank.Keys) {
                if(d.Contains(key)) {
                   var rank = SVGToRank[key];
                   return new Card(rank, color);
                }
            }
        }
        // For debug purposes
        Console.WriteLine("CARD RANK COULD NOT BE FOUND!!");
        var errorPath = new Random().Next();
        loc.ScreenshotAsync(new() { Path=$"./error/{errorPath}.png" });
        return new Card(0,0);
    }


    public static async Task SaveSvgImageCardPairs(IPage page) {
        List<CardSVGPair> pairs = new List<CardSVGPair>();
        Console.WriteLine("Trying to save board...");
        try { 
            pairs = InitPairs();
        } catch(Exception e) {
            Console.WriteLine(e.Message);
        }
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card");
        var cards = await cardsLoc.AllAsync();
        foreach(var card in cards) {
            var id = new Random().Next();
            var path = Path.Join(folder, $"{id}.png");
            try {
                await card.ScreenshotAsync(new() { Path = path, Timeout=2000 });
            } catch {
                Console.WriteLine("Couldn't take screenshot");
                continue; 
            }
            var svg = await card.Locator("path").AllAsync();
            ILocator p; 
            if(svg.Count < 2) {
                p  = svg[0];
            } else {
                p = svg[1];
            }
            string? attr = "";
            await p.GetAttributeAsync("d").ContinueWith((res) => {
                    attr = res.Result;
                    if(attr == null) {
                        return;
                    }
                    attr = CleanSVG(attr);
                    var data = new CardSVGPair(attr, path);
                    pairs?.Add(data);
                    Console.WriteLine("Added new data.");
                    });
        }
        File.WriteAllText(jsonPath, JsonSerializer.Serialize(pairs));
    }
    
    private static List<CardSVGPair> InitPairs() {
        string json = "[]";
        if(File.Exists(jsonPath)) {
            json = File.ReadAllText(jsonPath) ?? "[]";
        }
        List<CardSVGPair> pairs = JsonSerializer.Deserialize<List<CardSVGPair>>(json) ?? new();
        return pairs;
    }
    private static string CleanSVG(string attr) {
        var mRemover = new Regex("[Mm].*?(?=[A-Za-z])");
        attr = mRemover.Replace(attr, "");
        return attr;
    }

}
