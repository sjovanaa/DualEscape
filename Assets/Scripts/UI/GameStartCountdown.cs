using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameStartCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI timertext;
    [SerializeField] private TextMeshProUGUI cointext;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManagerr.Instance.onStateChanged += GameManagerr_onStateChanged;
        timertext.gameObject.SetActive(false);
        cointext.gameObject.SetActive(false);
    }

    private void GameManagerr_onStateChanged(object sender, System.EventArgs e)
    {
        if (GameManagerr.Instance.isCountdownToStartActive())
        {
            Show();
            

        }
        else
        {
            Hide();
            GlobalState.canMove = true;
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
        timertext.gameObject.SetActive(true);
        cointext.gameObject.SetActive(true);
    }

    private void Update()
    {
        text.text = Mathf.Ceil(GameManagerr.Instance.GetCountdownToStartTimer()).ToString();
    }
}
