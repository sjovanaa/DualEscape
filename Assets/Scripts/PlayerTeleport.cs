using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{
    [Tooltip("Objekat koji se teleportuje – obično je to Empty Object igrača")]
    public Transform player2;

    [Tooltip("PlayerMovement skripta koja kontroliše igrača. Ako nije postavljeno, pokušaće se automatsko pronalaženje.")]
    public PlayerMovement playerMovement;

    [Tooltip("Kratko odlaganje pre ponovnog omogućavanja kretanja")]
    public float teleportDelay = 0.1f;

    // Varijabla koja obezbeđuje da se teleport izvrši samo jednom tokom igre.
    private bool hasTeleported = false;

    void Start()
    {
        // Ako nije ručno postavljeno, pokušaj da izvučeš komponentu sa player2
        if (playerMovement == null && player2 != null)
        {
            playerMovement = player2.GetComponent<PlayerMovement>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement komponenta nije pronađena na player2 objektu!");
            }
        }
    }

    void Update()
    {
        // Ako je već izvršen teleport, ništa ne radimo.
        if (hasTeleported) return;

        // Koristi novi Input System za proveru da li je L pritisnuto
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            hasTeleported = true;
            StartCoroutine(TeleportRoutine());
        }
    }

    IEnumerator TeleportRoutine()
    {
        Debug.Log("Teleport rutina započeta.");

        // Onemogući skriptu za kretanje kako bi se nova pozicija ne prepisivala
        if (playerMovement != null)
        {
            Debug.Log("Isključujem PlayerMovement.");
            playerMovement.isTeleporting = true;
            playerMovement.enabled = false;
        }

        // Odredi nasumični offset: 50% šanse da se pomakne +15 jedinica u Z osi ili 50% šanse da se pomakne -15 jedinica u Z osi
        int randomChoice = Random.Range(0, 2);
        Vector3 offset = (randomChoice == 0) ? new Vector3(0, 0, 15) : new Vector3(0, 0, -15);
        Vector3 newPos = player2.position + offset;

        // Provera da li nova pozicija prelazi granicu lavirinta (Z ne sme da bude manji od 0)
        if (newPos.z < 0f)
        {
            Debug.Log("Teleportacija bi izvukla igrača van lavirinta. Koregujem poziciju.");
            newPos.z = 0f;
        }

        Debug.Log("Teleportujem: nova pozicija = " + newPos);
        player2.SetPositionAndRotation(newPos, player2.rotation);

        // Sačekaj kratko vreme da se pozicija "zaključi"
        yield return new WaitForSeconds(teleportDelay);

        // Ponovo omogući skriptu za kretanje – pošto teleport može biti korišćen samo jednom,
        // više ne vraćamo teleport flag pa se kontrola ostavlja samo za prvi teleport.
        if (playerMovement != null)
        {
            Debug.Log("Uključujem PlayerMovement.");
            playerMovement.enabled = true;
            playerMovement.isTeleporting = false;
        }

        Debug.Log("Teleport rutina završena.");
    }
}