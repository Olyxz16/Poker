using Microsoft.Playwright;
using Poker.Players;
using Poker.Online.Utils;
using Poker.Events;
using static Poker.IGameEvents;

namespace Poker.Online;

public class ReplayPokerGame : IGameEvents 
{
    
    private const string ENV_FILE = @"./.env";
    private const string BASE_URL = "https://www.replaypoker.com/";
    private const string ROOMS_URL = "https://www.replaypoker.com/lobby/rings/13366125";      
    
    private string mail = "";
    private string pass = "";
    private int target_price;
    private int max_free_seat;
    private bool debug;
        
    private Player player;
    private int round = -1;
    private bool playing = false;

    private bool running = true;
    private static Task? session;
    public static IPage? Page { get; private set; } 
    
    public event GameStartEventHandler? GameStartEvent;
    public event GameEndEventHandler? GameEndEvent;
    public event OnPlayerTurnEventHandler? OnPlayerTurnEvent;

    public delegate void OnTableCloseEventHandler(object sender);
    public static event OnTableCloseEventHandler? OnTableCloseEvent;
    // DEBUG
    public delegate Task BoardFullDEBUGEventHandler(IPage page, Player player);
    public static event BoardFullDEBUGEventHandler? BoardFullDEBUGEvent;


    public ReplayPokerGame(Player player, ReplayPokerGameConfig config) {
        this.player = player;
        InitConfig(config);
        OnPlayerTurnEvent += PlayerUtils.PlayTurn;
        OnTableCloseEvent += Reload;
        
        //BoardFullDEBUGEvent += CardUtils.GetGameStateFromBoard;
        //BoardFullDEBUGEvent += CardUtils.SaveSvgImageCardPairs;
    }
    public ReplayPokerGame(Player player) 
        : this(player, new()) {}

    private void InitConfig(ReplayPokerGameConfig config) {
        this.mail = config.MAIL;
        this.pass = config.PASS;
        this.target_price = config.TARGET_PRICE;
        this.max_free_seat = config.MAX_FREE_SEAT;
        this.debug = config.DEBUG;
        UpdateEnv();
        if(this.mail == null || this.mail == "") {
            mail = Environment.GetEnvironmentVariable("MAIL") ?? "";
        }
        if(this.pass == null || this.pass == "") {
            pass = Environment.GetEnvironmentVariable("PASS") ?? "";
        }
        
        if(this.mail == null || this.pass == null || this.mail == "" || this.pass == "") {
            throw new ArgumentException("You need to enter your ReplayPokerGames credentials.");
        }
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

    public async Task Play() {
        var browser = await Init();
        await Login(browser); 
        var room = await FetchRoom(browser, new() { Price = target_price, AvailableSeat = max_free_seat });
        Page = await JoinRoom(browser, room.URL);  

        session = Run(Page);
        await session;
    }
    private async void Reload(object sender) {
        if(Page == null) {
            return;
        }
        var context = Page.Context;
        var browser = context.Browser;
        await Page.CloseAsync();
        await context.CloseAsync();
        if(browser != null) await browser.CloseAsync();

        running = false;
        if(session == null) {
            return;
        }
        await session;

        running = true;
        await Play();
    }
    private async Task<IBrowserContext> Init() {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Firefox.LaunchAsync(new() {
            Headless = !debug
        });
        var context = await browser.NewContextAsync();
        return context;
    }
    private async Task Login(IBrowserContext browser) {
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
    private Task Run(IPage page) {
        // Horrendous but it works
        round = -1;
        playing = false;
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
                await WaitForWinner(page);
            }    
        });
        tasks.Add(task);
        task = Task.Run(async() => {
            while(running) {
                await WaitForBoardFullDebug(page);
            }
        });
        tasks.Add(task);
        return Task.WhenAll(tasks.ToArray());
    }
    private async Task WaitForNewTurn(IPage page) {
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card");
        while(round == 0 || await cardsLoc.CountAsync() > 0);
        round = 0;
        GameStartEvent?.Invoke(this, new GameStartEventArgs(new() { player }));
    }
    private async Task WaitForYourTurn(IPage page) {
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card");
        var playerLoc = page.Locator(".Seat.Seat--currentUser.Seat--currentPlayer");
        while(playing || await playerLoc.CountAsync() < 1);
        playing = true;

        var state = await CardUtils.GetGameStateFromBoard(page, player);
        OnPlayerTurnEvent?.Invoke(this, new OnPlayerTurnEventArgs(state));
        while(await playerLoc.CountAsync() >= 1);
        playing = false;
    }
    private async Task WaitForWinner(IPage page) {
        var winningLoc = page.Locator(".Stack--winnings");
        while(await winningLoc.CountAsync() == 0);
        var winnerPosClass = await winningLoc.GetAttributeAsync("class") ?? "";
        var winnerPos = PositionFromAttribute(winnerPosClass); 
        var playerLocClass = await page.Locator(".Seat--currentUser").GetAttributeAsync("class") ?? ""; 
        var playerPos = PositionFromAttribute(playerLocClass);
        
        bool playerWon = winnerPos == playerPos;
        var state = GameEndState.Win(new() { player }, playerWon ? player : null);
        GameEndEvent?.Invoke(this, new GameEndEventArgs(state));
    }
    private async Task WaitForClosedTable(IPage page) {
        var closedLoc = page.Locator("h1").Filter(new() { HasText="Table fermée" });
        while(await closedLoc.CountAsync() < 1);
        OnTableCloseEvent?.Invoke(this);
    }
    
    private async Task WaitForBoardFullDebug(IPage page) {
        var cardsLoc = page.Locator("div.Cards__communityCards").Locator("div.Card"); 
        while(round == 5 || await cardsLoc.CountAsync() < 5);
        round = 5;
        BoardFullDEBUGEvent?.Invoke(page, player);
    }
    
    private int PositionFromAttribute(string attr) {
        var classes = attr.Split(" ");
        var clazz = Array.Find(classes, str => str.StartsWith("Position--"));
        var target = classes.Last().ToString();
        return Int32.Parse(target);
    }

}
