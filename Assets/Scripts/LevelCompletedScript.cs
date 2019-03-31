using System.Collections;
using System.Collections.Generic;
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
            Destroy(GameObject.Find("NextLevel"));
            float canvasWidth = GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width;
            Vector3 textPosition = GameObject.Find("MainMenu").transform.position;
            GameObject.Find("MainMenu").transform.position = new Vector3(canvasWidth, textPosition[1], textPosition[2]);
        }

    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ButtonHandlerBackToMainMenu()
    {
        //Loads the Level Selection Menu
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
    }

    public void ButtonHandlerNext() {
        string nextLevel = "Level" + level;
        Debug.Log(nextLevel);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextLevel);
    }

    void WriteText() {
        GameObject.Find("CompletedText").GetComponent<Text>().text = "You completed Level " + (level-1) + "!";
    }

    public void HoverNoise()
    {
        GameObject.Find("HoverNoise").GetComponent<AudioSource>().Play();
    }
}
