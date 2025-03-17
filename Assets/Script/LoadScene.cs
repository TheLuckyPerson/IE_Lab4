using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    
    public void PlayGame(int selectedScene = -1)
    {
        int buildIdx = SceneManager.GetActiveScene().buildIndex + 1;
        if (selectedScene != -1) buildIdx = selectedScene;
        SceneManager.LoadScene(buildIdx % SceneManager.sceneCountInBuildSettings);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
