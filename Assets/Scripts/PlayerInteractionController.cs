using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct PlayerState
{
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

    private void Awake()
    {
        this.health = 100.0f;
    }

    void Update()
    {
        PhysicsRaycasts();
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource laserGun = GameObject.Find("LaserGunAudio").GetComponent<AudioSource>();
            laserGun.Play();
        }
    }

    void PhysicsRaycasts()
    {
        Vector3 centreOfScreen = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
        float distanceToFireRay = 20;
        Ray centreOfScreenRay = Camera.main.ScreenPointToRay(centreOfScreen);
        RaycastHit hit;
        if (Physics.Raycast(centreOfScreenRay, out hit, distanceToFireRay))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (hitObject.tag == "Enemy")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ToggleHitEnemyCursor());
                    //Debug.Log("Raycast hit: " + hit.transform.name);
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
    }

    void GraphicsRaycasts()    {
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
                /* change this to the function that you use to
                                                            change the cursor */
            }
        }
    }

    void ToggleSelectedCursor(bool interactable)
    {
        if (interactable)
        {
            crosshair.color = Color.yellow;
        }
        else {
            crosshair.color = Color.green;
        }
    }

    IEnumerator ToggleHitEnemyCursor() {
        crosshair.color = Color.red;
        yield return new WaitForSeconds(10f);
    }

    public void SetPlayerScore(int scoreUpdate){
        this.score += scoreUpdate;
    }

    public void getHit(float damageTaken) {
        health = this.health - damageTaken;
        SetPlayerHealth(health);
        UpdateHealthGUI();
        if (this.health <= 0) {
            SceneManager.LoadSceneAsync("Death");
        }
    }

    public void SetPlayerHealth(float health) {
        this.health = health;
        Text scoreHUD = GameObject.Find("HealthHUD").GetComponent<Text>();
        scoreHUD.text = "Health: " + this.health;
    }

    void UpdateHealthGUI()
    {
        Text scoreHUD = GameObject.Find("HealthHUD").GetComponent<Text>();
        scoreHUD.text = "Health: " + this.health;
    }

    void UpdateScoreGUI() {
        Text scoreHUD = GameObject.Find("ScoreHUD").GetComponent<Text>();
        scoreHUD.text = "Score: " + this.score;
    }

    public PlayerState ToRecord() {
        PlayerState state = new PlayerState(this.transform.position, this.transform.rotation, this.health, this.score);
        return state;
    }
}
