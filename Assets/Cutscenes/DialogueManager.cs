using System.Collections;
using TMPro;
using UnityEngine;

// This video helped with a lot of this script: https://www.youtube.com/watch?v=g609HLRKPXQ

public class DialogueManager : MonoBehaviour
{
    public TMP_Text text;
    public GameObject DialogueSystem;
    public GameObject NextIcon;
    [SerializeField] [TextArea] string desiredMessages;

    public float typewriterDelay = 0.05f;
    private bool isArmenian = false;

    // We hold this coroutine in a variable so that we are able to set it to null to manipulate if the player wants to
    // double click to skip past the dialogue and just display the full message immediately
    private Coroutine displayCoroutine;
    private string currentFullMessage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // This may look awkward, but the commas here allow us to write out all the messages we want in a confined space.
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
        NextIcon.SetActive(true);
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
            DialogueSystem.SetActive(false);
            NextIcon.SetActive(false);
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
                            isArmenian = true;
                        }
                        else if (tag.Contains("5C62D6"))
                        {
                            isArmenian = false;
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
                if (isArmenian)
                {
                    SoundManager.instance.soundPlayLetterArmenian(letter);
                } else
                {
                    SoundManager.instance.soundPlayLetterEnglish(letter);
                }
            }

            i++;
            yield return new WaitForSeconds(typewriterDelay);

        }
        displayCoroutine = null;
    }
}
