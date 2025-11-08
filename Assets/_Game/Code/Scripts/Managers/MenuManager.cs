using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject highscoresPanel;

    private void Start()
    {
        ShowMainMenuPanel();
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
}
