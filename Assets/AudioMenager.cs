using UnityEngine;

public class AudioMenager : MonoBehaviour
{
    [Header("--- Audio Source ------")]
    [SerializeField] AudioSource musicSource; 
    [SerializeField] AudioSource SFXSource; 

    [Header ("--- Audio Source ------")]
    public AudioClip background; 
    public AudioClip coin; 
    public AudioClip gameWin;
    public AudioClip doors;
    public AudioClip gameOver;

   private void Start()
   {
    musicSource.clip=background;
    musicSource.Play();
   }

   public void PlaySFX(AudioClip clip)
   {
    SFXSource.PlayOneShot(clip);
   }

}
