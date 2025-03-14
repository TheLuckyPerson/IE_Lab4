using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip nextEffect;
    [SerializeField] AudioClip[] alphabetArmenian;
    [SerializeField] AudioClip[] alphabetEnglish;
    [SerializeField] AudioClip[] alphabetFrench;

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

    public void soundPlayLetterArmenian(char letter)
    {
        letter = char.ToLower(letter);
        int index = letter - 'a';
        if (index >= 0 && index < alphabetArmenian.Length)
        {
            dialogueSource.PlayOneShot(alphabetArmenian[index]);
        }
    }

    public void soundPlayLetterEnglish(char letter)
    {
        letter = char.ToLower(letter);
        int index = letter - 'a';
        if (index >= 0 && index < alphabetEnglish.Length)
        {
            dialogueSource.PlayOneShot(alphabetEnglish[index]);
        }
    }

    public void soundPlayLetterFrench(char letter)
    {
        letter = char.ToLower(letter);
        int index = letter - 'a';
        if (index >= 0 && index < alphabetEnglish.Length)
        {
            dialogueSource.PlayOneShot(alphabetFrench[index]);
        }
    }
}
