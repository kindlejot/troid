using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance {  get; private set; }

    private const string MenuSceneName = "01_Menu";
    private const string GameSceneName = "02_Game";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad (gameObject);

            LoadInitialScenes();
        }
        else
        {
            Destroy (gameObject);
        }
    }

    private void LoadInitialScenes ()
    {
        SceneManager.LoadScene(MenuSceneName, LoadSceneMode.Additive);
        SceneManager.LoadScene(GameSceneName, LoadSceneMode.Additive);
    }

    public void StartGame ()
    {
        SceneManager.UnloadSceneAsync(MenuSceneName);

        GameManager.Instance.ChangeState(GameState.ResetGame);
    }

    public void LoadMenuScene ()
    {
        SceneManager.LoadScene(MenuSceneName, LoadSceneMode.Additive);
    }
}
