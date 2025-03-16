using TMPro;
using UnityEngine;

public class PlayTitle : MonoBehaviour
{
    public TextMeshProUGUI titleTextMesh;
    public string titleTextStr;
    private bool dialogueIsInScene = false;
    private DialogueManager dialogueContainer = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleTextMesh.text = titleTextStr;
        GameObject g = GameObject.FindGameObjectWithTag("Dialogue");
        if(g) {
            dialogueContainer = g.GetComponent<DialogueManager>(); 
        }

        if (dialogueContainer && dialogueContainer.isLevelCutscene)
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
