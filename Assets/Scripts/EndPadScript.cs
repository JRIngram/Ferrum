using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPadScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            int currentLevel = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel();
            GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(currentLevel + 1);
            Debug.Log(GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel());
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelComplete");
        }
    }
}
