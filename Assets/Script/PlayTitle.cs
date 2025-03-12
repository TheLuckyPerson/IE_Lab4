using TMPro;
using UnityEngine;

public class PlayTitle : MonoBehaviour
{
    public TextMeshProUGUI titleTextMesh;
    public string titleTextStr;
    private bool dialogueIsInScene = false;
    public GameObject dialogueContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        titleTextMesh.text = titleTextStr;
        dialogueContainer = GameObject.FindGameObjectWithTag("Dialogue");
        if (dialogueContainer)
        {
            dialogueIsInScene = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueIsInScene)
        {
            if (!dialogueContainer.activeInHierarchy)
            {
                titleTextMesh.gameObject.SetActive(true);
                dialogueIsInScene = false;
            }
        }
    }
}
