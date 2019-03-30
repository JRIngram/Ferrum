using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int level = 0;

    void Awake() {
        DontDestroyOnLoad(this);
    }

    public void UpdateLevel(int newLevel) {
        level = newLevel;
    }

    public int GetLevel() {
        return level;
    }

}
