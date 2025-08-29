using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI timertext;
    [SerializeField] private TextMeshProUGUI cointext;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button quitButton;
        private AudioMenager audioMenager;


    private void Awake()
    {           
        audioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();

        playAgainButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Time.timeScale = 1f;
    }
    void Start()
    {
        GameManagerr.Instance.onStateChanged += GameManagerr_onStateChanged;
        GameManagerr_onStateChanged(null, null);
        timertext.gameObject.SetActive(false);
        cointext.gameObject.SetActive(false);
    }

    private void GameManagerr_onStateChanged(object sender, System.EventArgs e)
    {
        if (GameManagerr.Instance.isGameOver() && GlobalState.gameEnded == false)
        {
            Show();
            audioMenager.PlaySFX(audioMenager.gameOver);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        timertext.gameObject.SetActive(false);
        cointext.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
