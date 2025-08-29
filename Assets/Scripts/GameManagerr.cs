using System;
using TMPro;
using UnityEngine;

public class GameManagerr : MonoBehaviour
{
    public static GameManagerr Instance { get; private set; }
    public event EventHandler onStateChanged;
    public event EventHandler onGamePaused;
    public event EventHandler onGameUnpaused;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer = 60f;
    private bool isGamePaused = false;
    [SerializeField] TextMeshProUGUI timerText;

    public float gettime()
    {
        return gamePlayingTimer;
    }
    public void settime(float addTime)
    {
        gamePlayingTimer += addTime;
    }

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.CountdownToStart;
                    onStateChanged?.Invoke(this,EventArgs.Empty);
                }
                break;

            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    onStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(gamePlayingTimer / 60);
                int seconds = Mathf.FloorToInt(gamePlayingTimer % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


                if (gamePlayingTimer <= 0.1f)
                {
                    state = State.GameOver;
                    onStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool isCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            onGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            onGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }


    public bool isGameOver()
    {
        return state == State.GameOver;
    }
}

