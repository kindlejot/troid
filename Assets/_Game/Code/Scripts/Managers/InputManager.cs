using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerControls _controls;

    public const string STATE_MENU = "Menu";
    public const string STATE_GAMEPLAY = "Gameplay";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _controls = new PlayerControls();

            SetInputState(STATE_MENU);

            _controls.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetInputState (string state)
    {
        _controls.Gameplay.Disable();
        _controls.UI.Disable();

        switch (state)
        {
            case STATE_MENU:
                _controls.UI.Enable();
                break;
            case STATE_GAMEPLAY:
                _controls.Gameplay.Enable();
                break;
            default:
                Debug.LogWarning ($"Attempted to set unknown input state: {state}");
                break;
        }
    }

    public PlayerControls.GameplayActions GetGameplayActions()
    {
        return _controls.Gameplay;
    }

    public PlayerControls.UIActions GetUIActions()
    {
        return _controls.UI;
    }

    private void OnDestroy()
    {
        _controls?.Disable();
        _controls?.Dispose();
    }
}
