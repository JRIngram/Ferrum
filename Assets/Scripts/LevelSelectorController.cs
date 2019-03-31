using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorController : MonoBehaviour
{
    public void ButtonHandlerBackToMainMenu()
    {
        //Loads the Level Selection Menu
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ButtonHandlerLevel1()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
    }

    public void ButtonHandlerLevel2()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(2);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level2");
    }

    public void ButtonHandlerLevel3()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(3);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level3");
    }

    public void HoverNoise()
    {
        GameObject.Find("HoverNoise").GetComponent<AudioSource>().Play();
    }
}
