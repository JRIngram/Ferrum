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
         * Runs the noiseThenLoadLevel coroutine and updates the LevelManager to give its level value the value of 1.
         */
        GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(1);
        StartCoroutine(NoiseThenLoadLevel());
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
        /**
         * Exits the game / application.
         */
        Application.Quit();
    }

    public void HoverNoise() {
        /**
         * Plays a noise when the player hovers over a button.
         */
        GameObject.Find("HoverNoise").GetComponent<AudioSource>().Play();
    }

    IEnumerator NoiseThenLoadLevel() {
        /**
         * Plays a gun noise and then loads the first level.
         */
        AudioSource playNoise = GameObject.Find("PlayNoise").GetComponent<AudioSource>();
        playNoise.Play();
        yield return new WaitForSeconds(playNoise.clip.length);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1");
    }
}
