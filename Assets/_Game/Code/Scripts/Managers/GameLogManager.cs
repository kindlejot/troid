using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameLogManager : MonoBehaviour
{
    public static GameLogManager Instance { get; private set; }

    public List<GameLogEntry> Log => _gameLogData.Entries;
    private GameLogData _gameLogData;

    private const string FILENAME = "gamelog.json";
    private string _savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _savePath = Path.Combine(Application.persistentDataPath, FILENAME);

            LoadLog();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEntry (string name, string description)
    {
        _gameLogData.Entries.Add(new GameLogEntry(name, description));
        SaveLog();
    }

    public void LoadLog()
    {
        if (File.Exists(_savePath))
        {
            try
            {
                string jsonString = File.ReadAllText(_savePath);
                _gameLogData = JsonUtility.FromJson<GameLogData>(jsonString);

                _gameLogData.Entries ??= new List<GameLogEntry>();
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to load GameLog: {exception.Message}. Initializating new data.");
                _gameLogData = new GameLogData();
            }
        }
        else
        {
            _gameLogData = new GameLogData();
        }
    }

    public void SaveLog()
    {
        string jsonString = JsonUtility.ToJson(_gameLogData, true);

        try
        {
            File.WriteAllText(_savePath, jsonString);
        }
        catch (System.Exception exception)
        {
            Debug.LogError($"Failed to save GameLog: {exception.Message}");
        }
    }
}
