using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip nextEffect;
    [SerializeField] AudioClip[] alphabet;

    [SerializeField] AudioSource dialogueSource;

    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void soundNextClick()
    {
        dialogueSource.PlayOneShot(nextEffect);
    }

    public void soundPlayLetter(char letter)
    {
        letter = char.ToLower(letter);
        int index = letter - 'a';
        if (index >= 0 && index < alphabet.Length)
        {
            dialogueSource.PlayOneShot(alphabet[index]);
        }
    }
}
