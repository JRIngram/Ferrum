using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveGameController saveManager = GameObject.Find("SaveManager").GetComponent<SaveGameController>();
        if (saveManager.GetLoadingLevel() == true) {
            saveManager.LoadEnemies();
        }
    }
}
