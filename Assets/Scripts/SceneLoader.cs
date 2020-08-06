using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {


    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
        //FindObjectOfType<GameStatus>().ResetGame();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLoseScene()
    {
        StartCoroutine(WaitABit());
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitABit()
    {

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}
