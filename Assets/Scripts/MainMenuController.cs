using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Controls the behaviour of the main menu.
 */

public class MainMenuController : MonoBehaviour
{
    public void Start()
    {
        /**
         * Ensures that the player can move the cursor.
         */
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ButtonHandlerPlay(){
        /**
         * Loads a new level.
         */
        GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(1);
        StartCoroutine(noiseThenLoadLevel());
    }

    public void ButtonHandlerLoad() {
        /**
         * Loads the saved level from the XML file.
         */
        GameObject.Find("SaveManager").GetComponent<SaveGameController>().LoadLevel();
    }

    public void ButtonHandlerLevel(){
		/**
         * Loads the Level Selection Menu
         */
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelSelection");
	}

    public void ButtonHandlerQuit()
    {
        Application.Quit();
    }

    public void HoverNoise() {
        GameObject.Find("HoverNoise").GetComponent<AudioSource>().Play();
    }

    IEnumerator noiseThenLoadLevel() {
        AudioSource playNoise = GameObject.Find("PlayNoise").GetComponent<AudioSource>();
        playNoise.Play();
        yield return new WaitForSeconds(playNoise.clip.length);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
    }
}
