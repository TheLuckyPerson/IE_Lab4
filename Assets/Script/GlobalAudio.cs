using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    private static GlobalAudio instance;
    private AudioSource audioSource;
    public AudioClip standardBackground;
    public AudioClip happyBackground;
    public AudioClip idkBackground;
    public AudioClip drumRoll;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.loop = true;
            audioSource.playOnAwake = false;

        } else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip newClip)
    {
        if (newClip == null)
        {
            Debug.LogError("PlayMusic() called with a null AudioClip!");
            return;
        }

        if (audioSource.clip == newClip) return;
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    public static GlobalAudio GetInstance()
    {
        if (instance == null)
        {
            Debug.Log("this was called");
            GameObject newAudioManager = new GameObject("GlobalAudio");
            instance = newAudioManager.AddComponent<GlobalAudio>();
            instance.audioSource = newAudioManager.AddComponent<AudioSource>();
            instance.audioSource.loop = true; 
            instance.audioSource.playOnAwake = false;
            DontDestroyOnLoad(newAudioManager);

            instance.LoadAudioClips();
        }
        return instance;
    }

    private void LoadAudioClips()
    {
        if (standardBackground == null)
            standardBackground = Resources.Load<AudioClip>("Audio/standardBackground");

        if (happyBackground == null)
            happyBackground = Resources.Load<AudioClip>("Audio/happyBackground");

        if (idkBackground == null)
            idkBackground = Resources.Load<AudioClip>("Audio/idkBackground");

        if (drumRoll == null)
            drumRoll = Resources.Load<AudioClip>("Audio/drumRoll");

        Debug.Log("Audio clips loaded from Resources folder.");
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume); 
    }
}
