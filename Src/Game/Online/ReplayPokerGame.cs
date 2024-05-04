using Microsoft.Playwright;
using Poker.Cards;
using Poker.Online.Events;
using Poker.Players;
using Poker.Online.Utils;

namespace Poker.Online;

public class ReplayPokerGame 
{
    
    private const string ENV_FILE = @"./.env";
    private const string BASE_URL = "https://www.replaypoker.com/";
    private const string ROOMS_URL = "https://www.replaypoker.com/lobby/rings/13366125";      
    
    private bool debug = true;
    private int target_price = 1;
    private int max_free_seat = 2;
        
    private Player player;
    private int round = -1;
    private bool playing = false;

    public delegate void GameStartEventHandler(object sender, GameStartEventArgs e);
    public static event GameStartEventHandler? GameStartEvent;
    public delegate void RoundStartEventHandler(object sender, RoundStartEventArgs e);
    public static event RoundStartEventHandler? RoundStartEvent;
    public delegate void OnPlayerTurnEventHandler(object sender, OnPlayerTurnEventArgs e);
    public static event OnPlayerTurnEventHandler? OnPlayerTurnEvent;

    // DEBUG
    public delegate Task BoardFullDEBUGEventHandler(IPage page);
    public static event BoardFullDEBUGEventHandler? BoardFullDEBUGEvent;


    public ReplayPokerGame(Player player) {
        this.player = player;

        BoardFullDEBUGEvent += CardUtils.SaveSvgImageCardPairs;
    }

    public async Task Play() {
        UpdateEnv();
        var browser = await Init();
        await Login(browser); 
        var room = await FetchRoom(browser, new() { Price = target_price, AvailableSeat = max_free_seat });
        var page = await JoinRoom(browser, room.URL);  

        Run(page);
    }
    private async Task<IBrowserContext> Init() {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Firefox.LaunchAsync(new() {
            Headless = !debug
        });
        var context = await browser.NewContextAsync();
        return context;
    }
    private void UpdateEnv() {
        if(System.IO.File.Exists(Path.GetFullPath(ENV_FILE))) {
            using (StreamReader reader = new StreamReader(ENV_FILE)) {
                String? line;
                while((line = reader.ReadLine()) != null) {
                    var pair = line.Split("=");
                    if(pair.Length != 2) {
                        continue;
                    }
                    System.Environment.SetEnvironmentVariable(pair[0], pair[1]);
                }
            }
       }
    }
    private async Task Login(IBrowserContext browser) {
        var mail = System.Environment.GetEnvironmentVariable("MAIL") ?? "";
        var pass = System.Environment.GetEnvironmentVariable("PASS") ?? "";

        var page = await browser.NewPageAsync();
        await page.GotoAsync(BASE_URL);
        await page.Locator("a").Filter(new() { HasText = "Log in" }).First.ClickAsync();
        await page.WaitForURLAsync("https://www.replaypoker.com/login");
        await page.Locator("#login").FillAsync(mail);
        await page.Locator("#password").FillAsync(pass);
        await page.Locator("button").Filter(new() { HasText = "Log in" }).ClickAsync();
        await page.CloseAsync();
    } 
    private async Task<Room> FetchRoom(IBrowserContext browser, RoomParams pars = new()) {
        var page = await browser.NewPageAsync();
        await page.GotoAsync(ROOMS_URL);
        AcceptCookies(page);
        //await CloseBlockingPane(page);
        var rooms = new List<Room>();
        await page.Locator("li.lobby-game").First.WaitForAsync();
        var els = await page.Locator("li.lobby-game").AllAsync();
        foreach(var el in els) {
            var holdem = (await el.Locator("div").Filter(new() { HasText="NL Hold'em" }).CountAsync()) > 0;
            if(!holdem) {
                continue;
            }
            var url = await el.Locator("a").GetAttributeAsync("href");
            if(url == null) {
                continue;
            }
            var seatText = await el.Locator("div.lobby-games-list-players").InnerTextAsync();
            var pair = seatText.Split(" / ");
            if(!(Int32.TryParse(pair[0], out int busySeat) && Int32.TryParse(pair[1], out int maxSeat))) {
                continue;
            }
            var open = busySeat < maxSeat;
            var blindText = await el.Locator("div.lobby-games-list-blinds").InnerTextAsync();
            if(!Int32.TryParse(blindText.Split(" / ")[0], out int blind)) {
                continue;
            }
            var room = new Room(url, open, blind, maxSeat, busySeat);
            rooms.Add(room);
        }
        await page.CloseAsync();
        if(pars.Price != null) {
            rooms = rooms.FindAll(elem => elem.Price == pars.Price);
        }
        if(pars.AvailableSeat != null) {
            rooms = rooms.FindAll(elem => elem.MaxSeat - elem.BusySeat < pars.AvailableSeat);
        }
        var result = rooms[new Random().Next(0, rooms.Count)];
        return result;
    }
    private void AcceptCookies(IPage page) {
        try {
            page.Locator("#onetrust-accept-btn-handler").ClickAsync();
        } catch(Exception e) {
            Console.Error.WriteLine(e);
        }
    }
    private async Task CloseBlockingPane(IPage page) {
        await page.Locator("#inApp-big-content").GetByLabel("Close").ClickAsync(); 
    }
    private async Task<IPage> JoinRoom(IBrowserContext browser, string url) {
        var page = await browser.NewPageAsync();
        await page.GotoAsync(url);
        //await page.Locator("div.SitNowControls").Locator("button").ClickAsync();
        //await page.Locator("button.RadioButton:nth-child(2)").ClickAsync();
        //await page.Locator("button").Filter(new() { HasText = "OK" }).ClickAsync();

        return page;
    }
    private void Run(IPage page) {
        GameStartEvent?.Invoke(this, new GameStartEventArgs());
        
        // Horrendous but it works
        var running = true;
        var tasks = new List<Task>();
        var task = Task.Run(async () => {
            while(running) {
                await WaitForNewTurn(page);
            }
        });
        tasks.Add(task);
        task = Task.Run(async () => {
            while(running) {
                await WaitForYourTurn(page);
            }
        });
        tasks.Add(task);
        task = Task.Run(async() => {
            while(running) {
                await WaitForBoardFullDebug(page);
            }
        });
        Task.WaitAll(tasks.ToArray());
    }
    private async Task WaitForNewTurn(IPage page) {
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card");
        while(round == 0 || await cardsLoc.CountAsync() > 0);
        round = 0;
        RoundStartEvent?.Invoke(this, new RoundStartEventArgs());
    }
    private async Task WaitForYourTurn(IPage page) {
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card");
        var playerLoc = page.Locator(".Seat.Seat--currentUser.Seat--currentPlayer");
        while(playing || await playerLoc.CountAsync() < 1);
        playing = true;

        var cards = new List<Card>();
        var cardsCountDEBUG = await cardsLoc.CountAsync();
        round = cardsCountDEBUG switch {
            0 => 1,
            3 => 2,
            4 => 3,
            5 => 4,
            _ => -1
        };
        var state = new GameState(round, 0, 0, player, new Dictionary<string, int>(), new List<Card>());
        OnPlayerTurnEvent?.Invoke(this, new OnPlayerTurnEventArgs(state));
    }

    private async Task WaitForBoardFullDebug(IPage page) {
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card"); 
        while(round == 5 || await cardsLoc.CountAsync() < 5);
        round = 5;
        BoardFullDEBUGEvent?.Invoke(page);
    }

}
