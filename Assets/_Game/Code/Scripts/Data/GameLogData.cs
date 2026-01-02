using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameLogEntry : ISerializationCallbackReceiver
{
    public string Name;
    public string Description;
    public string timestampStr; // For serialization
    public DateTime Timestamp;

    public GameLogEntry(string name, string description)
    {
        this.Name = name;
        this.Description = description;
        Timestamp = DateTime.Now;
    }

    public void OnBeforeSerialize()
    {
        timestampStr = Timestamp.ToString("o"); // ISO 8601 format
    }

    public void OnAfterDeserialize()
    {
        DateTime.TryParse(timestampStr, out Timestamp);
    }
}

[Serializable]
public class GameLogData
{
    public List<GameLogEntry> Entries = new List<GameLogEntry>();
}
