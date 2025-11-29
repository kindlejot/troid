using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState {
    Idle,
    ResetGame,
    NextLevel,
    Play,
    GameOver,
    Pause,
    Resume
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState;
    private GameState _previousState;

    public int Score;

    public ShipController Ship;
    [SerializeField] private ObstacleManager obstacles;
    [SerializeField] private GameObject scoreLabel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject gameOverScreen;

    // Class variables
    private PlayerControls.GameplayActions _gameplayActions; // For tracking "Pause" 
    private GameState _resumeState;

    private float _stateTime;

    private void Awake ()
    {
        if (Instance != null && Instance != this) {
            Destroy (this.gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("FATAL: InputManager not found in the scene! Controls can't be assigned.");
            return;
        }
        _gameplayActions = InputManager.Instance.GetGameplayActions();
        _gameplayActions.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        if (_gameplayActions.Pause != null)
        {
            _gameplayActions.Pause.performed -= OnPause;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (CurrentState != GameState.GameOver)
        {
            ChangeState(GameState.Pause);
        }
        else
        {
            ChangeState(GameState.Idle);
            SceneFlowManager.Instance.LoadMenuScene();
        }
    }

    void SetScore (int score)
    {
        Score = score;
        scoreText.text = Score.ToString();
    }

    public void AddPoints (int points)
    {
        SetScore (Score + points);
    }

    void Start ()
    {
        ChangeState (GameState.Idle);
    }

    public void ChangeState (GameState newState)
    {
        _previousState = CurrentState;
        CurrentState = newState;

        _stateTime = 0;

        switch (newState) {
            case GameState.Idle:
                Ship.gameObject.SetActive(false);
                gameOverScreen.SetActive (false);
                scoreLabel.SetActive (false);
                levelText.gameObject.SetActive (false);
                scoreText.gameObject.SetActive (false);
                obstacles.Reset();
                for (int i = 0; i < 5; i++)
                {
                    obstacles.NextLevel();
                }
                break;
            case GameState.ResetGame:
                gameOverScreen.SetActive (false);
                Ship.gameObject.SetActive (true);
                scoreLabel.SetActive(true);
                scoreText.gameObject.SetActive(true);
                Ship.Reset ();
                obstacles.Reset ();
                SetScore (0);
                ChangeState (GameState.NextLevel);
                break;
            case GameState.NextLevel:
                obstacles.NextLevel ();
                levelText.gameObject.SetActive (true);
                levelText.text =  "LEVEL " + obstacles.CurrentLevel;
                break;
            case GameState.Play:
                levelText.gameObject.SetActive (false);
                break;
            case GameState.GameOver:
                levelText.gameObject.SetActive (false);
                gameOverScreen.SetActive (true);
                Ship.gameObject.SetActive (false);
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                _resumeState = _previousState;
                SceneFlowManager.Instance.LoadMenuScene();
                break;
            case GameState.Resume:
                Time.timeScale = 1;
                ChangeState(_resumeState);
                break;
            default:
                throw new System.NotImplementedException ();          
        }
    }

    void UpdateState ()
    {
        _stateTime += Time.deltaTime;

        switch (CurrentState) {
            case GameState.NextLevel:
                if (_stateTime > 3) {
                    ChangeState (GameState.Play);
                }
                break;
            default:
                break;
        }
    }

    void Update ()
    {
        UpdateState ();
    }
}
