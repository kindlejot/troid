using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerControls controls;

    public const string STATE_MENU = "Menu";
    public const string STATE_GAMEPLAY = "Gameplay";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            controls = new PlayerControls();

            SetInputState(STATE_MENU);

            controls.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetInputState (string state)
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();

        switch (state)
        {
            case STATE_MENU:
                controls.UI.Enable();
                break;
            case STATE_GAMEPLAY:
                controls.Gameplay.Enable();
                break;
            default:
                Debug.LogWarning ($"Attempted to set unknown input state: {state}");
                break;
        }
    }

    public PlayerControls.GameplayActions GetGameplayActions()
    {
        return controls.Gameplay;
    }

    public PlayerControls.UIActions GetUIActions()
    {
        return controls.UI;
    }

    private void OnDestroy()
    {
        controls?.Disable();
        controls?.Dispose();
    }
}
