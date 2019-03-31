using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScript : MonoBehaviour
{
    int level = 0;
    void Awake()
    {
        level = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel();
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ButtonHandlerBackToMainMenu()
    {
        //Loads the Level Selection Menu
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ButtonHandlerRetry()
    {
        string retryLevel = "Level" + level;
        Debug.Log(retryLevel);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(retryLevel);
    }

    public void HoverNoise()
    {
        GameObject.Find("HoverNoise").GetComponent<AudioSource>().Play();
    }
}
