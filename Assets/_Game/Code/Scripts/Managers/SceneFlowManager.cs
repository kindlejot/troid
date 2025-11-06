using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    private const string MenuSceneName = "01_Menu";
    private const string GameSceneName = "02_Game";

    private void Awake()
    {
        LoadGameScenes();
    }

    private void LoadGameScenes ()
    {
        SceneManager.LoadScene(MenuSceneName, LoadSceneMode.Additive);
        SceneManager.LoadScene(GameSceneName, LoadSceneMode.Additive);
    }
}
