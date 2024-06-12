using UnityEngine;

public class SoundManagerInLogin : MonoBehaviour
{
    public AudioClip loopClip;
    public AudioClip buttonClickClip; // AudioClip para el sonido del botón

    private AudioSource audioSource;
    
    private float loopVolume = 0.2f;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        Invoke("PlayLoopAudio", 1f);
    }

    void PlayLoopAudio()
    {
        audioSource.clip = loopClip;
        audioSource.loop = true;
        audioSource.volume = loopVolume;
        audioSource.Play();
    }

    public void PlayButtonClickSound()
    {
        audioSource.Stop();
        audioSource.volume = 1f;
        audioSource.PlayOneShot(buttonClickClip);
        audioSource.volume = loopVolume;
        Invoke("PlayLoopAudio", 1f);
    }
}