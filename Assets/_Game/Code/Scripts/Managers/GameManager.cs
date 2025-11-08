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
    private GameState previousState;

    public int Score;

    public ShipController Ship;
    public ObstacleManager Obstacles;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LevelText;
    public GameObject GameOverScreen;
    public GameObject PauseScreen;

    float stateTime;

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
        ScoreText.text = Score.ToString();
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
        previousState = CurrentState;
        CurrentState = newState;

        stateTime = 0;

        switch (newState) {
            case GameState.Idle:
                GameOverScreen.SetActive (false);
                PauseScreen.SetActive (false);
                Ship.gameObject.SetActive (false);
                LevelText.gameObject.SetActive (false);
                ScoreText.gameObject.SetActive (false);
                Obstacles.Reset();
                for (int i = 0; i < 10; i++)
                {
                    Obstacles.NextLevel();
                }
                break;
            case GameState.ResetGame:
                GameOverScreen.SetActive (false);
                Ship.gameObject.SetActive (true);
                ScoreText.gameObject.SetActive(true);
                Ship.Reset ();
                Obstacles.Reset ();
                SetScore (0);
                ChangeState (GameState.NextLevel);
                break;
            case GameState.NextLevel:
                Obstacles.NextLevel ();
                LevelText.gameObject.SetActive (true);
                LevelText.text =  "LEVEL " + Obstacles.CurrentLevel;
                break;
            case GameState.Play:
                if (previousState == GameState.Pause)
                {
                    Time.timeScale = 1;
                }
                LevelText.gameObject.SetActive (false);
                PauseScreen.SetActive(false);
                break;
            case GameState.GameOver:
                LevelText.gameObject.SetActive (false);
                GameOverScreen.SetActive (true);
                Ship.gameObject.SetActive (false);
                break;
            case GameState.Pause:
                PauseScreen.SetActive (true);
                Time.timeScale = 0;
                break;
            default:
                throw new System.NotImplementedException ();          
        }
    }

    void UpdateState ()
    {
        stateTime += Time.deltaTime;

        switch (CurrentState) {
            case GameState.NextLevel:
                if (stateTime > 3) {
                    ChangeState (GameState.Play);
                }
                CheckForPause();
                break;
            case GameState.GameOver:
                if (stateTime > 5) {
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
