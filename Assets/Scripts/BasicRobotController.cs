using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Used to control the Boss's behaviour 
 */

public enum AgentState
{   
    /**
     * The three states the robot can be in:
     * 
     */
    Idle = 0,
    Patrolling,
    Chasing
}

[Serializable]
public struct RobotState {
    /**
     * Creates a RobotState struct, used for reloading robots.
     */
    public Vector3 position; //Position of the robot
    public Quaternion rotation; //Rotation of the robot
    public float health; //Health of the robot
    public AgentState state; //State of the robot

    public RobotState(Vector3 position, Quaternion rotation, float health, AgentState state) {
        this.position = position;
        this.rotation = rotation;
        this.health = health;
        this.state = state;
    }
}

public class BasicRobotController : MonoBehaviour
{
    public int scoreValue = 100;
    private float health = 100;
    public AgentState state;
    public Transform[] waypoints;
    public float damage;
    private NavMeshAgent navMeshAgent;
    private Animator animController;
    private int speedHashId;
    private int waypoint_id;
    private int attackHashId;
    private int next_waypoint;
    [SerializeField] private float distanceToStartHeadingToNextWaypoint;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToStartChasingTarget;
    [SerializeField] private float distanceToStartAttackingTarget;
    private RobotState robotState;
    public AudioClip punchSound;
    public AudioClip deathSound;

    void Awake()
    {
        /**
         * Creates the navMeshAgent and sets up the animator
         */
        speedHashId = Animator.StringToHash("walkingSpeed");
        attackHashId = Animator.StringToHash("attack");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
        this.target = GameObject.Find("Player").transform;
        if (waypoints.Length == 0)
            Debug.LogError("Error: list of waypoints is empty.");
        navMeshAgent.SetDestination(waypoints[0].position);
        distanceToStartAttackingTarget = navMeshAgent.stoppingDistance;
    }

    void Update()
    {
        /**
         * Checks distance from player: it will continue in either patrolling or idle state until the player is close.
         * Once the player is close the robot will chase the player until they are a large distance away.
         */
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= distanceToStartChasingTarget && angle < 180.0f && target.tag == "Player")
        {
            state = AgentState.Chasing;
            navMeshAgent.SetDestination(target.position);
            Chase();
        }
        else if (state == AgentState.Idle)
        {
            Idle();
        }
        else if (state == AgentState.Patrolling)
            Patrol();
        else {
            state = AgentState.Idle;
        }
    }

    void Chase()
    {
        /**
         * Follows the player until it is close enough to the player that it can attack.
         * If the player gets too far away the robot will move to an idle state.
         */
        animController.SetFloat(speedHashId, 10.0f);
        //Attack if close to player
        if (navMeshAgent.remainingDistance <= distanceToStartAttackingTarget)
        {
            navMeshAgent.speed = 0.0f;
            animController.SetFloat(speedHashId, 0.0f);
            animController.SetTrigger(attackHashId);
        }

        //Continue chasing if not close to player
        else {
            if (navMeshAgent.speed != 10.0f)
            {
                navMeshAgent.speed = 10.0f;
                animController.SetFloat(speedHashId, 10.0f);
            }
            navMeshAgent.isStopped = false;
        }

        if (navMeshAgent.remainingDistance > distanceToStartChasingTarget) {
            state = AgentState.Idle;
        }
    }

    void Idle()
    {
        /**
         * The robot stays in an idle position
         */
        animController.SetFloat(speedHashId, 0.0f);
        navMeshAgent.speed = 0.0f;
        navMeshAgent.isStopped = true;
        navMeshAgent.SetDestination(target.position);
    }

    void Patrol()
    {
        /**
         * The robot moves back and forth between two waypoints.
         */
        if (navMeshAgent.speed != 5.0f)
        {
            navMeshAgent.speed = 5.0f;
        }
        animController.SetFloat(speedHashId, 5.0f);
        navMeshAgent.isStopped = false;
        if (navMeshAgent.remainingDistance <= distanceToStartHeadingToNextWaypoint)
        {
            next_waypoint = (next_waypoint + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[next_waypoint].position);
        }
    }

    public void OnTriggerEnter(Collider other) {
        /**
         * If a player enters the robot's trigger collider then a punch sound is made and the player takes damage.
         */
        BoxCollider col = GetComponentInChildren<BoxCollider>();
        if (other.tag == "Player")
        {
            gameObject.GetComponent<AudioSource>().clip = punchSound;
            gameObject.GetComponent<AudioSource>().Play();
            other.GetComponent<PlayerInteractionController>().getHit(damage);
        }
    }


    public bool OnHit() {
        /**
         * If the robot is hit it loses 50 health; plays the death subroutine if the health is 0.
         */
        health = health - 50;
        if (health == 0)
        {
            StartCoroutine(PlayDeath());
            return true;
        }
        else {
            return false;
        }
    }

    public void SetHealth(float health) {
        /**
         * Mutator method to allow the robots health to be set.
         */
        this.health = health;
    }

    public float GetHealth() {
        /**
         * Accessor method to return the robots health.
         */
        return this.health;
    }

    public RobotState ToRecord() {
        /**
         * Creates a RobotState and returns it. Used for saving the player's state.
         */
        robotState = new RobotState(this.transform.position, this.transform.rotation, this.health, this.state);
        return robotState;
    }

    IEnumerator PlayDeath() {
        gameObject.GetComponent<AudioSource>().clip = deathSound;
        gameObject.GetComponent<AudioSource>().Play();       
        /*
         *  Destroys all children objects and colliders
         *  This gives the illusion that the object is destroyed whilst allowing the sound to play.
         *  Once audio completed the object is truly destroyed
        */
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Destroy(gameObject.GetComponent("BoxCollider"));
        Destroy(gameObject.GetComponent("CapsuleCollider"));
        yield return new WaitForSeconds(gameObject.GetComponent<AudioSource>().clip.length);
        Destroy(gameObject);
    }
}
