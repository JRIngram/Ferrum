using UnityEngine;

/**
 * Used to load a level from an XML file - called when the scene is loaded
 */

public class LevelStartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /**
         * Runs at the start of the level.
         */
        //If saveManager is in the loading level state then called the saveManager's LoadEntitites() method.
        SaveGameController saveManager = GameObject.Find("SaveManager").GetComponent<SaveGameController>();
        if (saveManager.GetLoadingLevel() == true) {
            saveManager.LoadEntities();
        }
        //Loads players score from LevelManager - the persistant score.
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteractionController>().SetPlayerScore(GameObject.Find("LevelManager").GetComponent<LevelManager>().score);
    }
}
