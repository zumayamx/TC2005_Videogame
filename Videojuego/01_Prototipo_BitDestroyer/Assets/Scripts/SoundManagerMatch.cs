using UnityEngine;

public class SoundManagerMatch : MonoBehaviour
{
    private AudioClip introClip;
    private AudioClip loopClip;
    private AudioClip buttonClickClip; // AudioClip para el sonido del botón

    private AudioClip selectCardClip; // AudioClip para el sonido de seleccionar una carta

    private AudioClip rouletteClip; // AudioClip para el sonido de la ruleta

    private AudioClip hitClip; // AudioClip para el sonido de golpe 

    private float loopVolume = 0.08f;

    private AudioSource audioSource;

    void Start()
    {
        introClip = Resources.Load<AudioClip>("Soundtracks/BeginMatchSound");
        loopClip = Resources.Load<AudioClip>("Soundtracks/OnLoopIntro");
        buttonClickClip = Resources.Load<AudioClip>("Soundtracks/OnClickSound");
        selectCardClip = Resources.Load<AudioClip>("Soundtracks/SoundSelectedCard");
        rouletteClip = Resources.Load<AudioClip>("Soundtracks/RouletteSound");
        hitClip = Resources.Load<AudioClip>("Soundtracks/hitSound");

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
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(buttonClickClip);
        audioSource.volume = loopVolume;
        Invoke("PlayLoopAudio", buttonClickClip.length + 2f);
    }

    public void PlaySelectCard () {
        audioSource.Stop();
        audioSource.volume = 1f;
        audioSource.PlayOneShot(selectCardClip);
        audioSource.volume = loopVolume;
        Invoke("PlayLoopAudio", selectCardClip.length + 2f);
    }

    public void SoundRoulette(float spinDuration) {
    // Detener el audio actual
    audioSource.Stop();

    // Asignar el clip de audio de la ruleta
    audioSource.clip = rouletteClip;

    // Ajustar el volumen
    audioSource.volume = 0.8f;

    // Calcular el punto de inicio del sonido de la ruleta
    float startTime = Mathf.Clamp(rouletteClip.length - spinDuration, 0, rouletteClip.length);

    // Reproducir el sonido de la ruleta desde el punto de inicio calculado
    audioSource.time = startTime;
    audioSource.Play();

    // Programar el audio para detenerse cuando el clip termine
    Invoke("StopRouletteSound", spinDuration);

    // Restaurar el volumen del loop después de que el sonido de la ruleta termine
    Invoke("PlayLoopAudio", spinDuration + 3f);
    }

    private void StopRouletteSound() {
    audioSource.Stop();
    audioSource.volume = loopVolume;
    }

    public void PlayHitSound()
    {
        audioSource.Stop();
        audioSource.volume = 0.8f;
        audioSource.PlayOneShot(hitClip);
        audioSource.volume = loopVolume;
        Invoke("PlayLoopAudio", hitClip.length + 10f);
    }
   
   // public void AttackSound 
}