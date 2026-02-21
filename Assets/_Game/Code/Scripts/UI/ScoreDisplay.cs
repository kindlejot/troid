using System;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numbers;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highscoreColorBright;
    [SerializeField] private Color highscoreColorDark;
    
    private bool _isHighscore;

    private void OnEnable()
    {
        GameManager.OnScoreChanged += ScoreChanged;
        _isHighscore = false;
        ScoreChanged(0);

        numbers.color = defaultColor;
    }

    private void OnDisable()
    {
        GameManager.OnScoreChanged -= ScoreChanged;
    }

    private void ScoreChanged(int score)
    {
        // To avoid constant check against list, stop checking when we reach highscore
        if (!_isHighscore && HighScoreManager.Instance != null && HighScoreManager.Instance.IsHighscore(score))
        {
            _isHighscore = HighScoreManager.Instance.IsHighscore(score);
        }

        numbers.text = score.ToString();
    }

    private void Update()
    {
        if (_isHighscore)
        {
            numbers.color = Color.Lerp(highscoreColorBright, highscoreColorDark, Mathf.Sin(Time.time * Mathf.PI * 2));
        }
    }
}
