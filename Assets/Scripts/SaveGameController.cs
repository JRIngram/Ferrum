using System;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class SaveGameController : MonoBehaviour
{

    [SerializeField]
    private GameObject basicEnemyRobot;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject boss;

    private const string SAVEGAME_FILE = "Assets/Saves/ferrum-savegame.xml";

    private bool loadingLevel;

    [Serializable]
    public struct GameState
    {
        public RobotState[] robotStates;
        public PlayerState playerState;
        public BossState bossState;
        public int level;

        public GameState(RobotState[] robotStates, int level, PlayerState playerState, BossState bossState)
        {
            this.robotStates = robotStates;
            this.level = level;
            this.playerState = playerState;
            this.bossState = bossState;
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
                if (robots[i].GetHealth() > 0.0f) {
                    states[i] = robots[i].ToRecord();
                }
            }
            PlayerState playerState = GameObject.Find("Player").GetComponent<PlayerInteractionController>().ToRecord();
            BossState bossState = GameObject.Find("Boss").GetComponent<BossController>().ToRecord();
            GameState gs = new GameState(states, level, playerState, bossState);
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
                GameObject.Find("LevelManager").GetComponent<LevelManager>().UpdateLevel(level);
            }
        }
        string levelString = "Level" + level;
        SceneManager.LoadSceneAsync(levelString);
    }

    public void LoadEntities() {
        LoadEnemies();
        LoadPlayer();
        this.loadingLevel = false;
        Debug.Log("DONE LOADING!");
    }

    void LoadEnemies() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }

        foreach (RobotState state in this.gameState.robotStates)
        {
            GameObject newRobot = Instantiate(basicEnemyRobot);
            newRobot.transform.position = state.position;
            newRobot.transform.rotation = state.rotation;
            newRobot.GetComponent<BasicRobotController>().setHealth(state.health);
            newRobot.GetComponent<BasicRobotController>().state = state.state;
            newRobot.GetComponent<NavMeshAgent>().Warp(new Vector3(newRobot.transform.position.x, newRobot.transform.position.y + 1, newRobot.transform.position.z));
        }

        Destroy(GameObject.FindGameObjectWithTag("Boss"));
        if (gameState.bossState.health > 0)
        {
            GameObject newBoss = Instantiate(boss);
            newBoss.transform.position = gameState.bossState.position;
            newBoss.transform.rotation = gameState.bossState.rotation;
            newBoss.GetComponent<BossController>().setHealth(gameState.bossState.health);
            newBoss.GetComponent<NavMeshAgent>().Warp(new Vector3(newBoss.transform.position.x, newBoss.transform.position.y + 1, newBoss.transform.position.z));
        }
    }

    void LoadPlayer() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        player.transform.position = this.gameState.playerState.position;
        player.transform.rotation = this.gameState.playerState.rotation;
        player.GetComponent<PlayerInteractionController>().SetPlayerScore(this.gameState.playerState.score);
        player.GetComponent<PlayerInteractionController>().SetPlayerHealth(this.gameState.playerState.health);
        StartCoroutine(WaitToActivate());
    }

    public bool GetLoadingLevel() {
        return this.loadingLevel;
    }

    IEnumerator WaitToActivate()
    {   
        //Waits two frames before reactivating script so that the character is loaded into the correct position.
        print(Time.time);
        yield return new WaitForSeconds(Time.deltaTime * 2);
        print(Time.time);
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;
    }
}
