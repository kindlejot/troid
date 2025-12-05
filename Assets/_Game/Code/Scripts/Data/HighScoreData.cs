using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ScoreEntry
{
    public string Initials;
    public int Score;

    public ScoreEntry (string initials, int score)
    {
        this.Initials = initials;
        this.Score = score;
    }
}

[Serializable]
public class HighScoreData
{
    public List<ScoreEntry> Scores = new List<ScoreEntry>();
}
