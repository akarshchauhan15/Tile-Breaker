using System;
using System.Globalization;

public class ScoreEntry
{
    public int Score { get; set; }
    public DateTime Date { get; set; }

    public ScoreEntry(int score)
    {
        Score = score;
        Date = DateTime.Now;
    }
}