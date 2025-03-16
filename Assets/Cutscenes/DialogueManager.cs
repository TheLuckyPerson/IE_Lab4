using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This video helped with a lot of this script: https://www.youtube.com/watch?v=g609HLRKPXQ

public class DialogueManager : MonoBehaviour
{
    public TMP_Text text;
    public GameObject DialogueSystem;
    [SerializeField] [TextArea] string desiredMessages;
    [SerializeField] Image fadeImage;
    [SerializeField] RawImage speakerImage;
    [SerializeField] Texture2D fabianoMugshot;
    [SerializeField] Texture2D adelardMugshot;
    [SerializeField] Texture2D wizardMugshot;
    private MonoBehaviour playerScript;
    public float typewriterDelay = 0.05f;
    enum Language { Armenian, English, French};
    private Language currentLanguage;
    [SerializeField] bool isLevelCutscene = false;

    // We hold this coroutine in a variable so that we are able to set it to null to manipulate if the player wants to
    // double click to skip past the dialogue and just display the full message immediately
    private Coroutine displayCoroutine;
    private string currentFullMessage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerScript = player.GetComponent<PlayerScript>();
        }

        if (SceneManager.GetActiveScene().name == "Ctsc1")
        {
            StartCoroutine(WaitForCatIdle());
        } else if (SceneManager.GetActiveScene().name == "Ctsc2") {
            StartCoroutine(PlayCtsc2());
        } else if (SceneManager.GetActiveScene().name == "Ctsc3")
        {
            GlobalAudio.GetInstance().StopMusic();
            StartCoroutine(FadeInCoroutine());
            ShowMessage(desiredMessages);
        }
        else if (isLevelCutscene) 
        {
            ShowMessage(desiredMessages);
            if (playerScript != null)
            {
                playerScript.enabled = false;
            }
        }
        
    }

    private IEnumerator PlayCtsc2()
    {
        Ctsc2Play.instance.isPanningCamera = true;
        GlobalAudio.GetInstance().PlayMusic(GlobalAudio.GetInstance().drumRoll);
        yield return new WaitForSeconds(3f);
        GlobalAudio.GetInstance().StopMusic();
        ShowMessage(desiredMessages);
    }

    private IEnumerator WaitForCatIdle()
    {
        Ctsc1Play.instance.catAnimation = true;
        GlobalAudio.GetInstance().SetVolume(0.5f);
        GlobalAudio.GetInstance().PlayMusic(GlobalAudio.GetInstance().drumRoll);
        yield return new WaitUntil(() => Ctsc1Play.instance.CatIsIdle);
        Ctsc1Play.instance.catAnimation = false;
        GlobalAudio.GetInstance().StopMusic();
        yield return new WaitForSeconds(1.5f);
        ShowMessage(desiredMessages);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // If the message is still typing, but our player wants to finish it immediately, this if will allow them to do that.
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
                text.text = currentFullMessage;
                displayCoroutine = null;
            }
            // If no message is currently playing (the couroutine is not being run and is null) then we can move on to the
            // next message in the list.
            else
            {
                if (number != 0)
                {
                    Skip();
                    SoundManager.instance.soundNextClick();
                }
            }
        }
    }

    // Will hold all the strings that we split via comma from the start() function
    string[] words;
    int number;
    public void ShowMessage(string Message)
    { 
        // Initially prompts the display of the first message in the star
        number = 0;
        words = Message.Split(',');
        DialogueSystem.SetActive(true);
 
        Skip();
    }

    public void Skip()
    {
        if (number < words.Length)
        {
            currentFullMessage = words[number];
            displayCoroutine = StartCoroutine(TypeText(currentFullMessage));
            number += 1;
        } else
        {
            number = 0;
            if (playerScript != null)
            {
                playerScript.enabled = true;
            }

            if (SceneManager.GetActiveScene().name == "Ctsc1" || 
                SceneManager.GetActiveScene().name == "Ctsc2" || 
                SceneManager.GetActiveScene().name == "Ctsc3")
            {
                StartCoroutine(FadeOutCoroutine());
                
            }
            DialogueSystem.SetActive(false);
        }
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        if (SceneManager.GetActiveScene().name == "Ctsc2")
        {
            Ctsc2Play.instance.catAnimation = true;
            yield return new WaitForSeconds(2f);
        }
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / 1f);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        int buildIdx = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(buildIdx);
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        fadeImage.color = new Color(color.r, color.g, color.b, 1f);

        yield return new WaitForSeconds(1f);
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (elapsedTime / 1f)); 
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

       
    }

    private IEnumerator TypeText(string message)
    {
        text.text = "";

        int i = 0;

        while (i < message.Length)
        {
            if (message[i] == '<')
            {
                int closeTag = message.IndexOf('>', i);
                if (closeTag != -1)
                {
                    string tag = message.Substring(i, closeTag - i + 1);
                    text.text += tag;

                    if (tag.StartsWith("<color=", System.StringComparison.OrdinalIgnoreCase))
                    {
                        if (tag.Contains("FC6C85"))
                        {
                            currentLanguage = Language.Armenian;
                            speakerImage.texture = fabianoMugshot;
                        }
                        else if (tag.Contains("5C62D6"))
                        {
                            currentLanguage = Language.English;
                            speakerImage.texture = adelardMugshot;
                        } else
                        {
                            currentLanguage = Language.French;
                            speakerImage.texture = wizardMugshot;
                        }
                        
                    }

                    i = closeTag + 1;
                    continue;
                }
            }

            char letter = message[i];
            text.text += letter;

            if(char.IsLetter(letter))
            {
                switch (currentLanguage)
                {
                    case Language.Armenian:
                        SoundManager.instance.soundPlayLetterArmenian(letter);
                        break;
                    case Language.English:
                        SoundManager.instance.soundPlayLetterEnglish(letter);
                        break;
                    case Language.French:
                        SoundManager.instance.soundPlayLetterFrench(letter);
                        break;
                }
            }

            i++;
            yield return new WaitForSeconds(typewriterDelay);

        }
        displayCoroutine = null;
    }

    public void SkipAllDialogue()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        text.text = ""; 
        number = words.Length; 

        if (playerScript != null)
        {
            playerScript.enabled = true; 
        }

        DialogueSystem.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Ctsc1" ||
            SceneManager.GetActiveScene().name == "Ctsc2" ||
            SceneManager.GetActiveScene().name == "Ctsc3")
        {
            StartCoroutine(FadeOutCoroutine());
        }
    }

}
