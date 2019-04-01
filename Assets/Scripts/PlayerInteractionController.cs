using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

/**
 *  Attached to the Player object. This script controls the players interactions with the game.
 */

[Serializable]
public struct PlayerState
{
    /**
     * Stores the players position, rotation, health and score. Used to save the player's state so that it can reloaded later.
     */
    public Vector3 position;
    public Quaternion rotation;
    public float health;
    public int score;

    public PlayerState(Vector3 position, Quaternion rotation, float health, int score)
    {
        this.position = position;
        this.rotation = rotation;
        this.health = health;
        this.score = score;
    }
}


public class PlayerInteractionController : MonoBehaviour {
    public Sprite crosshairImage;
    public Sprite crosshairSelectable;
    public Image crosshair;
    public GraphicRaycaster graphicRaycaster;
    private int score;
    private float health;
    private bool hitEnemy; //Checks if the enemy has been hit.

    private void Awake()
    {
        /**
         * Sets the players health upon starting the script.
         */
        this.health = 100.0f;
    }

    void Update()
    {
        /**
         * Checks if player can/has hit an enemy and makes a laser gun noise if the player presses their left mouse button. 
         */
        PhysicsRaycasts();
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource laserGun = GameObject.Find("LaserGunAudio").GetComponent<AudioSource>();
            laserGun.Play();
        }
    }

    void PhysicsRaycasts()
    {
        /**
         * Raycasts to check if player is facing enemy or boss.
         * Toggles cursor if enemy/boss is being faced.
         * If player pressed left mouse button when facing enemy/boss then the enemy/boss is damaged and score update appropriately.
         */
        Vector3 centreOfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        float distanceToFireRay = 20;
        Ray centreOfScreenRay = Camera.main.ScreenPointToRay(centreOfScreen);
        RaycastHit hit;
        if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.tag == "Enemy" || hitObject.tag == "Boss")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hitObject.tag == "Enemy")
                    {
                        bool enemyKilled = hitObject.GetComponent<BasicRobotController>().onHit();
                        if (enemyKilled)
                        {
                            SetPlayerScore(hitObject.GetComponent<BasicRobotController>().scoreValue);
                        }
                        Text scoreHUD = GameObject.Find("ScoreHUD").GetComponent<Text>();
                        scoreHUD.text = "Score: " + this.score;
                    }
                    if (hitObject.tag == "Boss") {
                        bool enemyKilled = hitObject.GetComponent<BossController>().onHit();
                        if (enemyKilled)
                        {
                            SetPlayerScore(hitObject.GetComponent<BossController>().scoreValue);
                        }
                        Text scoreHUD = GameObject.Find("ScoreHUD").GetComponent<Text>();
                        scoreHUD.text = "Score: " + this.score;
                    }
                }
                else {
                    ToggleSelectedCursor(true);
                }
            }
            else
            {
                ToggleSelectedCursor(false);
            }
        }
        else
        {
            ToggleSelectedCursor(false);
        }

        if (Input.GetKeyDown("q")) {
            SceneManager.LoadSceneAsync("MainMenu");
        }
    }

    void GraphicsRaycasts(){
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);
        bool hitButton = false;
        if(results.Count > 0)
        {
            for(int i = 0; i < results.Count; i++)
            {
                Button button = results[i].gameObject.GetComponent<Button>();
                if(button != null)
                {
                    hitButton = true;
                    if(Input.GetMouseButtonDown(0)) button.onClick.Invoke();
                }
            }
            if(hitButton)
            {
               ToggleSelectedCursor(true);
            }
        }
    }

    void ToggleSelectedCursor(bool interactable)
    {
        /**
         * Changes the colour of the crosshair:
         *      * Green if object is not interactable.
         *      * Yellow if object is interactable.
         */
        if (interactable)
        {
            crosshair.color = Color.yellow;
        }
        else {
            crosshair.color = Color.green;
        }
    }

    public void SetPlayerScore(int scoreUpdate){
        /**
         * Updates the player's score and then updates the GUI.
         */
        this.score += scoreUpdate;
        UpdateScoreGUI();
    }

    public int GetPlayerScore() {
        /**
         * Returns the player's score
         */
        return this.score;
    }

    public void getHit(float damageTaken) {
        /**
         * Lowers the player's health by damage taken. If health is equal to or less than 0 then the Death scene is loaded.
         */
        health = this.health - damageTaken;
        SetPlayerHealth(health);
        UpdateHealthGUI();
        if (this.health <= 0) {
            SceneManager.LoadSceneAsync("Death");
        }
    }

    public void SetPlayerHealth(float health) {
        /**
         * Sets the player's health and updates the GUI.
         */
        this.health = health;
        Text scoreHUD = GameObject.Find("HealthHUD").GetComponent<Text>();
        scoreHUD.text = "Health: " + this.health;
    }

    void UpdateHealthGUI()
    {
        /**
         * Updates the health section of GUI.
         */
        Text scoreHUD = GameObject.Find("HealthHUD").GetComponent<Text>();
        scoreHUD.text = "Health: " + this.health;
    }

    void UpdateScoreGUI() {
        /**
         * Updates the score section of the GUI.
         */
        Text scoreHUD = GameObject.Find("ScoreHUD").GetComponent<Text>();
        scoreHUD.text = "Score: " + this.score;
    }

    public PlayerState ToRecord() {
        /**
         * Creates a PlayerState object and returns this object.
         */
        PlayerState state = new PlayerState(this.transform.position, this.transform.rotation, this.health, this.score);
        return state;
    }
}
