using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    
    public void PlayGame()
    {
        int buildIdx = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(buildIdx);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
