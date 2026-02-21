using System.Globalization;
using TMPro;
using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] GameObject scoreContainer;
    [SerializeField] GameObject scoreRowPrefab;

    private void OnEnable()
    {
        if (scoreContainer == null || scoreRowPrefab == null)
        {
            Debug.LogError("ScoreContainer or ScoreRowPrefab not assigned to HighScoreDisplay script");
            return;
        }

        Populate();
    }

    private void Populate()
    {
        if (scoreContainer.transform.childCount > 0)
        {
            foreach (Transform child in scoreContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (HighScoreManager.Instance != null)
        {
            int position = 1;

            NumberFormatInfo nfi = new NumberFormatInfo {
                NumberGroupSeparator = ",",
                NumberDecimalDigits = 0 };

            foreach (ScoreEntry entry in HighScoreManager.Instance.HighScores)
            {
                // 01:ABC.....1,234,567

                string positionString = $"{position,2:N0}:".Replace(' ', '0');
                string scoreString = entry.Score.ToString("N", nfi);

                GameObject obj = Instantiate(scoreRowPrefab, scoreContainer.transform);
                obj.GetComponent<TextMeshProUGUI>().text = positionString + entry.Initials + scoreString.PadLeft(14, '.');
                position++;
            }
        }
    }
}
