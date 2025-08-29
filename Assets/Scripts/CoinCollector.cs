using TMPro;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private GameManagerr gm; 
    private AudioMenager audioMenager;
    private int coin = 0;

    public TextMeshProUGUI coinText;

    private void Awake()
    {
        audioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coins"))
        {
            gm.settime(3f);
            audioMenager.PlaySFX(audioMenager.coin);
            coin++;
            coinText.text = "Coins: " + coin.ToString();
            Debug.Log(coin);
            Destroy(other.gameObject);
        }
    }
}
