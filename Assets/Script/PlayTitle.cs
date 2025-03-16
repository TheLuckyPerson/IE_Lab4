using TMPro;
using UnityEngine;

public class PlayTitle : MonoBehaviour
{
    public TextMeshProUGUI titleTextMesh;
    public string titleTextStr;
    private bool dialogueIsInScene = false;
    public DialogueManager dialogueContainer;
    private GameObject dialogueBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleTextMesh.text = titleTextStr;
        dialogueContainer = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueManager>();
        if (dialogueContainer.isLevelCutscene)
        {
            dialogueIsInScene = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueIsInScene) {
            if (dialogueContainer.finishedLevel) {
                titleTextMesh.gameObject.SetActive(true);
                dialogueIsInScene = false;
            }
        } else {
            titleTextMesh.gameObject.SetActive(true);
        }
    }
}
