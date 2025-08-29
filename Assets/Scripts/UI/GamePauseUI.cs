using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        GameManagerr.Instance.onGamePaused += GameManagerr_OnGamePaused;
        GameManagerr.Instance.onGameUnpaused += GameManagerr_OnGameUnpaused;
        Hide();
    }

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
           
            GameManagerr.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void GameManagerr_OnGameUnpaused(object sender, EventArgs e)
    {
        Hide();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void GameManagerr_OnGamePaused(object sender, EventArgs e)
    {
        Show();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
