using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPadScript : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            Debug.Log("Player has stayed in the the building!!!");
            Debug.Log("Level completed!!!");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        }
    }
}
