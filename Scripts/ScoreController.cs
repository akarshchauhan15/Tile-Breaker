using Godot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

public partial class ScoreController : Node
{
    public static List<ScoreEntry> Scores = new List<ScoreEntry>();
    public static string Path = "res://scores.json";

    public static void AddScores(int Score)
    {
        Scores.Add(new ScoreEntry(Score));
        Scores = Scores.OrderByDescending(entry => entry.Score).Take(10).ToList();

        SaveScores();
    }
    public static void LoadScores()
    {
        if (!FileAccess.FileExists(Path))
            return;
        
        FileAccess File = FileAccess.Open(Path, FileAccess.ModeFlags.Read);
        string Json = File.GetAsText();
        File.Close();

        Scores = JsonSerializer.Deserialize<List<ScoreEntry>>(Json);
    }
    public static Tuple<string, string> GetScoreListString()
    {
        if (Scores.Count == 0)
            return Tuple.Create("", "");

        string ScoreListText = "";
        string DateListText = "";

        DateTime Today = DateTime.Now;

        foreach (ScoreEntry Score in ScoreController.Scores)
        {
            string DisplayDate;
            int Difference = Today.Day - Score.Date.Day;

            switch (Difference)
            {
                case 0:
                    DisplayDate = "Today";
                    break;
                case 1:
                    DisplayDate = "Yesterday";
                    break;
                default:
                    DisplayDate = Score.Date.ToString("dd MMM", new CultureInfo("en-GB"));
                    break;
            }

            ScoreListText += $"{Score.Score}\n";
            DateListText += $"{DisplayDate}\n";
        }

        return Tuple.Create(ScoreListText, DateListText);
    }
    private static void SaveScores()
    {
        var Json = JsonSerializer.Serialize(Scores);
        FileAccess File = FileAccess.Open(Path, FileAccess.ModeFlags.Write);
        File.StoreString(Json);
        File.Close();
    }
}