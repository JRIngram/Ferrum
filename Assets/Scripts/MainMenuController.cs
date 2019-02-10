using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void ButtonHandlerPlay(){
		//Will load a new level!
		//AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
	}
	
	public void ButtonHandlerLevel(){
		//Loads the Level Selection Menu
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelSelection");
	}
}
