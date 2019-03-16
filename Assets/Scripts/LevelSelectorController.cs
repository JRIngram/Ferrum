﻿using System.Collections;
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
    }

    public void ButtonHandlerLevel2()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level2");
    }

    public void ButtonHandlerLevel3()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level3");
    }
}
