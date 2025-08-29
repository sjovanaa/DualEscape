using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLightToggleTimed : MonoBehaviour
{
    [Tooltip("Referenca na spotlight iznad igrača koji će biti kontrolisan.")]
    public Light playerSpotlight;

    [Tooltip("Trajanje u sekundama koliko će svetlo ostati aktivno (pre nego se automatski isključi).")]
    public float lightUsageDuration = 15f;

    // Ova promenljiva prati da li je svetlo trenutno aktivno.
    private bool isLightActive = false;

    void Start()
    {
        // Uveri se da je svetlo isključeno na početku.
        if (playerSpotlight != null)
        {
            playerSpotlight.enabled = false;
        }
        else
        {
            Debug.LogError("Nije postavljena referenca na spotlight!");
        }
    }

    void Update()
    {
        // Koristi novi Input System za detekciju pritiska tastera T.
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            // Ako svetlo nije trenutno aktivno, pokrećemo korutinu koja ga uključuje.
            if (!isLightActive)
            {
                StartCoroutine(ActivateLightRoutine());
            }
        }
    }

    IEnumerator ActivateLightRoutine()
    {
        // Uključi svetlo i postavi flag da je aktivno.
        isLightActive = true;
        playerSpotlight.enabled = true;
        Debug.Log("Svetlo je uključeno.");

        // Sačekaj zadato trajanje dok je svjetlo normalno aktivno.
        yield return new WaitForSeconds(lightUsageDuration);

        // Nakon isteka trajanja, simuliraj kraj flashlightra:
        // 1. Ugasi svjetlo na 1 sekundu.
        playerSpotlight.enabled = false;
        Debug.Log("Svetlo ugaseno (blink off) na 1 sekundu.");
        yield return new WaitForSeconds(1f);

        // 2. Upali svjetlo na 1 sekundu.
        playerSpotlight.enabled = true;
        Debug.Log("Svetlo uključeno (blink on) na 1 sekundu.");
        yield return new WaitForSeconds(1f);

        // 3. Na kraju trajno ugasi svjetlo.
        playerSpotlight.enabled = false;
        Debug.Log("Svetlo je trajno ugašeno nakon blinking efekta.");

        // Resetuj flag, tako da se opet može aktivirati pritiskom tastera T.
        isLightActive = false;
    }
}