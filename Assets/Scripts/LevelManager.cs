using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Stores the level that the player is currently. on
 */ 

public class LevelManager : MonoBehaviour
{
    public int level = 0;

    void Awake() {
        /**
         * Ensures the object is not destroyed between scenes.
         */
        DontDestroyOnLoad(this);
    }

    public void UpdateLevel(int newLevel) {
        /**
         * Mutator method to set the level stored by the Level manager.
         */
        level = newLevel;
    }

    public int GetLevel() {
        /**
         * Accessor method to get the level of the Level manager.
         */ 
        return level;
    }

}
