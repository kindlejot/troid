using TMPro;
using UnityEngine;

public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI finalScoreElement;
    [SerializeField] GameObject newHighScorePanel;
    [SerializeField] GameObject continueButton;

    private int _finalScore;
    private bool _isNewHighscore;
    private string _initials;

    public void Initialize (int finalScore)
    {
        _finalScore = finalScore;
        _isNewHighscore = HighScoreManager.Instance.IsHighscore(finalScore);

        finalScoreElement.text = finalScore.ToString();
        newHighScorePanel.SetActive(_isNewHighscore);
        continueButton.SetActive(!_isNewHighscore);
    }

    public void InitialsChanged(string initials)
    {
        continueButton.SetActive(initials.Length > 0);
        _initials = initials;
    }

    public void ContinuePressed()
    {
        if (_isNewHighscore)
        {
            HighScoreManager.Instance.AddScore(_initials.PadRight(3, '.'), _finalScore);
        }
    }
}
