using System;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SaveGameController : MonoBehaviour
{

    [SerializeField]
    private GameObject basicEnemyRobot;

    private const string SAVEGAME_FILE = "Assets/Saves/ferrum-savegame.xml";

    private bool loadingLevel;

    [Serializable]
    public struct GameState
    {
        public RobotState[] robotStates;
        public int level;

        public GameState(RobotState[] robotStates, int level)
        {
            this.robotStates = robotStates;
            this.level = level;
        }
    }

    private GameState gameState;

    private void Awake()
    {
        loadingLevel = false;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save(SAVEGAME_FILE);
        }
    }

    void Save(string filename) {
        String sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1" || sceneName == "Level2" || sceneName == "Level3")
        {
            Debug.Log("Saving scene\t" + sceneName);
            BasicRobotController[] robots = FindObjectsOfType<BasicRobotController>();
            int level = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel();
            RobotState[] states = new RobotState[robots.Length];
            for (int i = 0; i < robots.Length; i++)
            {
                states[i] = robots[i].ToRecord();
            }

            GameState gs = new GameState(states, level);
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, gs);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(filename);
            }
            print("saved.");
        }
        else {
            Debug.Log("Not a valid level to save on");
        }
    }

    public void LoadLevel() {
        Debug.Log("Loading...");
        this.loadingLevel = true;
        string filename = SAVEGAME_FILE;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        string xmlString = xmlDocument.OuterXml;

        RobotState[] states;
        int level;

        using (StringReader read = new StringReader(xmlString))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            using (XmlReader reader = new XmlTextReader(read))
            {
                this.gameState = (GameState)serializer.Deserialize(reader);
                states = gameState.robotStates;
                level = gameState.level;
            }
        }
        string levelString = "Level" + level;
        SceneManager.LoadSceneAsync(levelString);
    }

    public void LoadEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Debug.Log("KILL KILL KILL");
            Destroy(enemies[i]);
        }

        foreach (RobotState state in this.gameState.robotStates)
        {
            GameObject newRobot = Instantiate(basicEnemyRobot);
            newRobot.transform.position = state.position;
            newRobot.transform.rotation = state.rotation;
            newRobot.GetComponent<BasicRobotController>().setHealth(state.health);
            newRobot.GetComponent<BasicRobotController>().state = state.state;
        }

        this.loadingLevel = false;
    }

    public bool GetLoadingLevel() {
        return this.loadingLevel;
    }
}
