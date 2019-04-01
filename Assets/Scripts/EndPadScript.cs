using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Used to complete the level.
 * This should be attached to the end_level_pad.
 */ 

public class EndPadScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        /**
         * If the colliding object is the player then load the next level.
         */
        if(other.tag == "Player") {
            int currentLevel = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel();
            GameObject.Find("LevelManager").GetComponent<LevelManager>().score = GameObject.Find("Player").GetComponent<PlayerInteractionController>().GetPlayerScore();
            GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(currentLevel + 1);
            Debug.Log(GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel());
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelComplete");
        }
    }
}
