using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip introClip;
    public AudioClip loopClip;
    public AudioClip buttonClickClip; // AudioClip para el sonido del botón

    private float loopVolume = 0.3f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (introClip != null && loopClip != null)
        {
            audioSource.clip = introClip;
            audioSource.volume = 0.7f;
            audioSource.Play();
            Invoke("PlayLoopAudio", introClip.length);
        }
        else
        {
            Debug.LogError("AudioClips no asignados en el inspector.");
        }
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
    }
}