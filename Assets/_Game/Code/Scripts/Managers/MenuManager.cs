using UnityEngine;
using System.ComponentModel;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject highscoresPanel;

    [Header("Play and Resume buttons")] // Content switching based of the GameState when menu was loaded
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject resumeButton;

    private void Start()
    {
        ShowMainMenuPanel();

        // For activating "Resume" button
        if (GameManager.Instance != null)
        {
            playButton.SetActive(GameManager.Instance.CurrentState != GameState.Pause);
            resumeButton.SetActive(GameManager.Instance.CurrentState == GameState.Pause);
        }
    }

    // Panel navigation
    public void ShowMainMenuPanel ()
    {
        mainMenuPanel.SetActive (true);
        settingsPanel.SetActive(false);
        highscoresPanel.SetActive (false);
    }

    public void ShowSettingsPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        highscoresPanel.SetActive(false);
    }

    public void ShowHighscoresPanel()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        highscoresPanel.SetActive(true);
    }

    public void QuitGame ()
    {
        Application.Quit ();

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }

    public void PlayGame ()
    {
        SceneFlowManager.Instance.StartGame();
    }

    public void ResumeGame ()
    {
        SceneFlowManager.Instance.ResumeGame();
    }
}
