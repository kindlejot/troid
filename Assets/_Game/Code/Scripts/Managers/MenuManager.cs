using UnityEngine;
using System.ComponentModel;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject highscoresPanel;
    [SerializeField] private GameOverDisplay gameOverPanel;

    [Header("Play and Resume buttons")] // Content switching based of the GameState when menu was loaded
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject resumeButton;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

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

    private void OpenPanel(GameObject targetPanel)
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        highscoresPanel.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);

        if (targetPanel != null) targetPanel.SetActive(true);
    }

    // Panel navigation
    public void ShowMainMenuPanel() => OpenPanel(mainMenuPanel);
    public void ShowSettingsPanel() => OpenPanel(settingsPanel);
    public void ShowHighscoresPanel() => OpenPanel(highscoresPanel);

    public void ShowGameOverPanel(int finalScore)
    {
        OpenPanel(gameOverPanel.gameObject);
        gameOverPanel.Initialize(finalScore);
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
