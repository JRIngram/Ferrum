using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompletedScript : MonoBehaviour
{

    int level = 0;
    void Awake()
    {
        level = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel();
        WriteText();
        if (level == 4) {
            /**
             * If the next level is level 4 then move the main menu button to the centre and remove the next level button.
             */
            Destroy(GameObject.Find("NextLevel"));
            float canvasWidth = GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width;
            Vector3 textPosition = GameObject.Find("MainMenu").transform.position;
            GameObject.Find("MainMenu").transform.position = new Vector3(canvasWidth, textPosition[1], textPosition[2]);
        }

    }

    void Start()
    {
        /**
         * Ensures that the player can move their cursor once the LevelComplete scene is loaded.
         */
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ButtonHandlerBackToMainMenu()
    {
        /**
         * Loads the Level Selection Menu
         */
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ButtonHandlerNext() {
        /**
         * Loads the next level.
         */ 
        string nextLevel = "Level" + level;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
    }

    void WriteText() {
        /**
         * Changes text in the centre of the LevelCompleted canvas.
         */
        GameObject.Find("CompletedText").GetComponent<Text>().text = "You completed Level " + (level-1) + "!";
    }

    public void HoverNoise()
    {
        /**
         * Plays a noise when the player hovers over a button.
         */
        GameObject.Find("HoverNoise").GetComponent<AudioSource>().Play();
    }
}
