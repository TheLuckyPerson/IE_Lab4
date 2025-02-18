using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    public GameObject player;  // assign the player
    public GameObject winScreen;  // assign the win panel

    void Start()
    {
        winScreen.SetActive(false);
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        GetComponent<Animator>().Play("levelcomplete");

        if (player != null)
        {
            player.GetComponent<PlayerScript>().canMove = false; // disable playe movement
        }
    }

    public void LoadNextLevel()
    {
        int buildIdx = SceneManager.GetActiveScene().buildIndex + 1;
        if (buildIdx >= SceneManager.sceneCountInBuildSettings)
        {
            buildIdx = 0;
        }
        SceneManager.LoadScene(buildIdx);
    }
}