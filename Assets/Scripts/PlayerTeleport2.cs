using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport2 : MonoBehaviour
{
    [Tooltip("Objekat koji se teleportuje – obično je to Empty Object igrača")]
    public Transform player2;

    [Tooltip("PlayerMovement skripta koja kontroliše igrača. Ako nije postavljeno, pokušaće se automatsko pronalaženje.")]
    public PlayerMovementArrowKeys playerMovement;

    [Tooltip("Kratko odlaganje pre ponovnog omogućavanja kretanja")]
    public float teleportDelay = 0.1f;

    // Varijabla koja osigurava da se teleportacija izvrši samo jednom
    private bool hasTeleported = false;

    void Start()
    {
        // Ako playerMovement nije ručno postavljen, pokušaj da izvučeš komponentu sa player2
        if (playerMovement == null && player2 != null)
        {
            playerMovement = player2.GetComponent<PlayerMovementArrowKeys>();
            if (playerMovement == null)
            {
                Debug.LogError("PlayerMovement komponenta nije pronađena na player2 objektu!");
            }
        }
    }

    void Update()
    {
        if (hasTeleported) return;

        // Koristi novi Input System da detektuje pritisak dugmeta G
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            hasTeleported = true;
            StartCoroutine(TeleportRoutine());
        }
    }

    IEnumerator TeleportRoutine()
    {
        Debug.Log("Teleport rutina započeta.");

        // Onemogući skriptu za kretanje kako bi se osigurala korektna pozicija
        if (playerMovement != null)
        {
            Debug.Log("Isključujem PlayerMovement.");
            playerMovement.isTeleporting = true;
            playerMovement.enabled = false;
        }

        // Izaberi nasumični offset: 50% šanse da se pomeri +15 u Z osi ili -15 u Z osi
        int randomChoice = Random.Range(0, 2);
        Vector3 offset = (randomChoice == 0) ? new Vector3(0, 0, 15) : new Vector3(0, 0, -15);
        Vector3 newPos = player2.position + offset;

        // Ograniči novu Z koordinatu na opseg [0, 117]
        newPos.z = Mathf.Clamp(newPos.z, 0f, 117f);

        Debug.Log("Teleportujem: nova pozicija = " + newPos);
        player2.SetPositionAndRotation(newPos, player2.rotation);

        // Sačekaj kratko vreme da se pozicija "zaključi"
        yield return new WaitForSeconds(teleportDelay);

        // Ponovo omogući skriptu za kretanje
        if (playerMovement != null)
        {
            Debug.Log("Uključujem PlayerMovement.");
            playerMovement.enabled = true;
            playerMovement.isTeleporting = false;
        }

        Debug.Log("Teleport rutina završena.");
    }
}