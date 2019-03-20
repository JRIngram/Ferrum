using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static int level = 0;

    void Awake() {
        DontDestroyOnLoad(this);
    }

    public void UpdateLevel(int newLevel) {
        level = newLevel;
        Debug.Log("LEVEL NOW " + level);
    }

}
