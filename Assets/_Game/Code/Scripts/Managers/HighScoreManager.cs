using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }

    public List<ScoreEntry> HighScores => _highScoreData.Scores;
    private HighScoreData _highScoreData;

    private const string FILENAME = "highscores.json";
    private string _savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _savePath = Path.Combine(Application.persistentDataPath, FILENAME);

            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScores()
    {
        if (File.Exists(_savePath))
        {
            try
            {
                string jsonString = File.ReadAllText(_savePath);
                _highScoreData = JsonUtility.FromJson<HighScoreData>(jsonString);

                if (_highScoreData.Scores == null)
                {
                    _highScoreData.Scores = new List<ScoreEntry>();
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"Failed to load Highscores: {exception.Message}. Initializating new data.");
                _highScoreData = new HighScoreData();
            }
        }
        else
        {
            _highScoreData = new HighScoreData();

            InitializeDefaultScores();
        }
    }

    public void SaveScores()
    {
        string jsonString = JsonUtility.ToJson(_highScoreData, true);

        try
        {
            File.WriteAllText(_savePath, jsonString);
        }
        catch (System.Exception exception)
        {
            Debug.LogError ($"Failed to save Highscores: {exception.Message}");
        }
    }

    private void InitializeDefaultScores()
    {
        for (int i=0; i<10; i++)
        {
            _highScoreData.Scores.Add(
                new ScoreEntry(new string((char)('A' + i), 3), // AAA, BBB, ... JJJ
                               10000 - (i * 1000)));
        }

        SaveScores();
    }
}
