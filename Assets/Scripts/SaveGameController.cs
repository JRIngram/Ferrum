using System;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class SaveGameController : MonoBehaviour
{

    [SerializeField]
    private GameObject basicEnemyRobot;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject boss;

    private const string SAVEGAME_FILE = "Assets/Saves/ferrum-savegame.xml"; //File path for save game file.

    private bool loadingLevel; //Used to indicate if a level is being loaded.

    [Serializable]
    public struct GameState
    {
        /**
         * Creates a GameState struct which stores the level, state of all robots, state of the player (score/health) and state of the boss. 
         */
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
        /**
         * Puts the object in a non-level loading state and causes object not to be destroy when new scenes are loaded.
         */
        loadingLevel = false;
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        /**
         * If F1 is pressed then the game is saved.
         */
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Save(SAVEGAME_FILE);
        }
    }

    void Save(string filename) {
        /**
         * Saves the boss state, player state and robot states into an XML file.
         */
        String sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1" || sceneName == "Level2" || sceneName == "Level3")
        {
            Debug.Log("Saving scene\t" + sceneName);
            int level = GameObject.Find("LevelManager").GetComponent<LevelManager>().GetLevel();
            
            //For each robot create a RobotState object and add it to states.
            BasicRobotController[] robots = FindObjectsOfType<BasicRobotController>();
            RobotState[] states = new RobotState[robots.Length];
            for (int i = 0; i < robots.Length; i++)
            {
                if (robots[i].GetHealth() > 0.0f) {
                    states[i] = robots[i].ToRecord();
                }
            }
           
            //Create a PlayerState object.
            PlayerState playerState = GameObject.Find("Player").GetComponent<PlayerInteractionController>().ToRecord();
            
            //Create a boss state object.
            BossState bossState = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>().ToRecord();
            
            //Add robot state, the current level, playerState and bossState to a GameState object.
            GameState gs = new GameState(states, level, playerState, bossState);
            
            //Create XML serializer
            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, gs);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(filename);
            }
            Debug.Log("saved.");
        }
        else {
            Debug.Log("Not a valid level to save on");
        }
    }

    public void LoadLevel() {
        /**
         * Loads a level state and player's score state from an XML file
         */
        Debug.Log("Loading...");
        this.loadingLevel = true; //Sets loadingLevel state to true.

        //Loads XML file
        string filename = SAVEGAME_FILE;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(filename);
        string xmlString = xmlDocument.OuterXml;
        
        //Loads RobotStates and level.
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
        /**
         * Loads enemies and the player and then sets the loadingLevel state to false.
         */
        LoadEnemies();
        LoadPlayer();
        this.loadingLevel = false;
    }

    void LoadEnemies() {
        /**
         * Loads the robots and boss into the game:
         *      * Robot: position, rotation, health and state.
         *      * Boss: position, rotation, health.
         */
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
        /**
         *  Loads the players position, rotation, score and health.  
         */
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
        player.transform.position = this.gameState.playerState.position;
        player.transform.rotation = this.gameState.playerState.rotation;
        player.GetComponent<PlayerInteractionController>().SetPlayerScore(this.gameState.playerState.score);
        player.GetComponent<PlayerInteractionController>().SetPlayerHealth(this.gameState.playerState.health);
        StartCoroutine(WaitToActivate());
    }

    public bool GetLoadingLevel() {
        /**
         * Returns the loadingLevel state of the object.
         */
        return this.loadingLevel;
    }

    IEnumerator WaitToActivate()
    {   
        /**
         * Waits two frames before reactivating FirstPersonController script so that the character is loaded into the correct position.
         */
        yield return new WaitForSeconds(Time.deltaTime * 2);
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = true;
    }
}
