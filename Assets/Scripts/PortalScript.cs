using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private HashSet<GameObject> playersInPortal = new HashSet<GameObject>();
    [SerializeField] private GameObject gameSuccessScreen;
    private AudioMenager audioMenager;

   
    private void Start()
    {
        gameSuccessScreen.SetActive(false);
    }
     private void Awake()
    {
        audioMenager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioMenager>();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInPortal.Add(other.gameObject); // Dodaje se samo jednom, jer HashSet ne dozvoljava duplikate

            if (playersInPortal.Count == 2)
            {

                gameSuccessScreen.SetActive(true);
                audioMenager.PlaySFX(audioMenager.gameWin);
                GlobalState.gameEnded = true;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0f;
                // Ovde pozovi kraj igre ili neku scenu ili UI
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInPortal.Remove(other.gameObject);
        }
    }


}

