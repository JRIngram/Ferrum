using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ButtonHandlerPlay(){
        //Will load a new level!
        GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(1);
        StartCoroutine(noiseThenLoadLevel());
    }

    public void ButtonHandlerLoad() {
        GameObject.Find("SaveManager").GetComponent<SaveGameController>().LoadLevel();
    }

    public void ButtonHandlerLevel(){
		//Loads the Level Selection Menu
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelSelection");
	}

    public void ButtonHandlerQuit()
    {
        Application.Quit();
    }

    IEnumerator noiseThenLoadLevel() {
        AudioSource playNoise = GameObject.Find("PlayNoise").GetComponent<AudioSource>();
        playNoise.Play();
        yield return new WaitForSeconds(playNoise.clip.length);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
    }
}
