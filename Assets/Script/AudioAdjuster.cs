using UnityEngine;

public class AudioAdjuster : MonoBehaviour
{
    [SerializeField] bool standard;
    [SerializeField] bool happy;
    [SerializeField] bool idk;

    private void Awake()
    {
        GlobalAudio audioManager = GlobalAudio.GetInstance();
        GlobalAudio.GetInstance().SetVolume(0.1f);
        if (standard)
        {
            GlobalAudio.GetInstance().PlayMusic(audioManager.standardBackground);
        } else if (happy)
        {
            GlobalAudio.GetInstance().PlayMusic(audioManager.happyBackground);
        } else if (idk) 
        {
            GlobalAudio.GetInstance().PlayMusic(audioManager.idkBackground);
        }
    }
}
