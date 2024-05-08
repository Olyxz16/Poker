using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Poker.Cards;

namespace Poker.Online.Utils;

public class CardAnalyzer {
    
    private const string inputPath = @"./cards/raw.json";
    private const string outputPath = @"./cards/output.json";
    
    private const int CONTINUE = 0;
    private const int REPEAT = 1;
    private const int EXIT = 2;
    private const int ERROR = 3;
    
    private static Dictionary<char, int> CardRank = new Dictionary<char, int>() {
        {'1', 1},
        {'2', 2},
        {'3', 3},
        {'4', 4},
        {'5', 5},
        {'6', 6},
        {'7', 7},
        {'8', 8},
        {'9', 9},
        {'X', 10},
        {'J', 11},
        {'Q', 12},
        {'K', 13}
    };
    private static Dictionary<char, CardColor> CardCol = new Dictionary<char, CardColor>() {
        {'C', CardColor.HEART},
        {'K', CardColor.DIAMOND},
        {'P', CardColor.CLUB},
        {'T', CardColor.SPADE}
    };

    private List<CardSVGPair> inputs;
    private List<CardSVGPair> outputs;


    public CardAnalyzer() {
        
        try {
            inputs = JsonSerializer.Deserialize<List<CardSVGPair>>(File.ReadAllText(inputPath)) ?? new List<CardSVGPair>();
            CleanInputs();
        } catch {
            inputs = new List<CardSVGPair>();
        }
        try {
            outputs = JsonSerializer.Deserialize<List<CardSVGPair>>(File.ReadAllText(outputPath)) ?? new List<CardSVGPair>();
        } catch {
            outputs = new List<CardSVGPair>();
        }
    }

    public void Run() {
        
        KeepProcessForeground();

        for(int i = 0 ; i < inputs.Count ; i++) {
            var input = inputs[i];
            if(outputs.Any(val => val.ImagePath == input.ImagePath)) {
                continue;
            }
            var process = OpenImage(input.ImagePath);
            var val = Console.ReadLine() ?? "";
            CloseImage(process);
            var flag = ParseInput(val, input, out var output);
            if(flag == 0) {
                TryAdd(output);
            } else if(flag == 1) {
                i--;
            } else if(flag == 2) {
                break;
            } else if(flag == 3) {
                File.Delete(input.ImagePath);
                inputs.Remove(input);
            }
        }
        
        ExitAndSave();

    }

    private void KeepProcessForeground() {
        var processes = Process.GetProcessesByName("Tabby");
        Task.Run(() => {
            while(true) {
                var handle = processes[0].MainWindowHandle;
                SetForegroundWindow(handle);
            }
        });
    }

    private void CleanInputs() {
        inputs = inputs.DistinctBy(card => card.ImagePath).ToList();
        var result = new List<CardSVGPair>(inputs.Count);
        foreach(var pair in inputs) {
            if(File.Exists(Path.GetFullPath(pair.ImagePath))) {
                result.Add(pair);
            } 
        }
        inputs = result;
    }

    private void TryAdd(CardSVGPair pair) {
        Console.WriteLine(pair.Card?.Rank + " " + pair.Card?.Color);
        int index = outputs.FindIndex(v => v.Card == pair.Card && v.Card?.Color == pair.Card?.Color);
        bool outputContainsCard = index != -1;
        if(outputContainsCard) {
            var other = outputs[index];
            if(other.SVG == pair.SVG) {
                Console.WriteLine("Card dupe, similar.");
            } else {
                Console.WriteLine("Card dupe, DIFF SVG !!");
            }
        }
        outputs.Add(pair);
        Console.WriteLine("Added card");
    }
    
    private int ParseInput(string val, CardSVGPair from, out CardSVGPair to) {
        to = new CardSVGPair("", "");
        if(val == "exit") {
            return EXIT;
        }
        if(val == "error") {
            return ERROR;
        }
        if(val.Length != 2) {
            Console.WriteLine("Wrong input.");
            return REPEAT;
        }
        (char rank, char col) = (val[0], val[1]);
        if(!CardRank.ContainsKey(rank)) {
            Console.WriteLine("Wrong input : rank.");
            return REPEAT; 
        }
        if(!CardCol.ContainsKey(col)) {
            Console.WriteLine("Wrong input : color");
            return REPEAT;
        }
        var card = new Card(CardRank[rank], CardCol[col]);
        to = new CardSVGPair(card, from.SVG, from.ImagePath);
        return CONTINUE;
    }

    private void ExitAndSave() {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(outputPath, JsonSerializer.Serialize(outputs, options));
    }


    [DllImport("shell32.dll")]
    static extern int FindExecutable(string lpFile, string lpDirectory, [Out] StringBuilder lpResult);
    [DllImport("User32")]
    private static extern int SetForegroundWindow(IntPtr hwnd);
    [DllImportAttribute("User32.DLL")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public Process OpenImage(string imagePath)
    {

        var exePath = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        var arguments = Path.GetFullPath(imagePath);

        var process = new Process();
        process.StartInfo.FileName = exePath;
        process.StartInfo.Arguments = arguments;

        process.Start();
        return process;
    }
    private void CloseImage(Process process) {
        process.Kill();
        process.Close();
    } 




}
