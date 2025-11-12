using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState {
    Idle,
    ResetGame,
    NextLevel,
    Play,
    GameOver,
    Pause
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
    [SerializeField] private GameObject pauseScreen;

    float _stateTime;

    private void Awake ()
    {
        if (Instance != null && Instance != this) {
            Destroy (this.gameObject);
        } else {
            Instance = this;
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
                pauseScreen.SetActive (false);
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
                if (_previousState == GameState.Pause)
                {
                    Time.timeScale = 1;
                }
                levelText.gameObject.SetActive (false);
                pauseScreen.SetActive(false);
                break;
            case GameState.GameOver:
                levelText.gameObject.SetActive (false);
                gameOverScreen.SetActive (true);
                Ship.gameObject.SetActive (false);
                break;
            case GameState.Pause:
                pauseScreen.SetActive (true);
                Time.timeScale = 0;
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
                CheckForPause();
                break;
            case GameState.GameOver:
                if (_stateTime > 5) {
                    if (Input.GetButtonDown ("Fire1")) {
                        ChangeState (GameState.Idle);
                        SceneFlowManager.Instance.LoadMenuScene();
                    }
                }
                break;
            case GameState.Play:
                CheckForPause();
                break;
            case GameState.Pause:
                CheckForPause();
                break;
            default:
                break;
        }
    }

    void CheckForPause ()
    {
        if (Input.GetButtonUp("Pause"))
        {
            ChangeState((CurrentState != GameState.Pause) ? GameState.Pause : GameState.Play);
        }
    }

    void Update ()
    {
        UpdateState ();
    }
}
