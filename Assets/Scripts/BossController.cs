using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public struct BossState
{
    /**
     * Used to save the Boss's state. 
     */

    public Vector3 position; //Position of the boss
    public Quaternion rotation; //Rotation of the boss
    public float health; //Boss's health
    public bool spottedPlayer; //Whether the boss has spotted the player.

    public BossState(Vector3 position, Quaternion rotation, float health, bool spottedPlayer)
    {
        this.position = position;
        this.rotation = rotation;
        this.health = health;
        this.spottedPlayer = spottedPlayer;
    }
}

public class BossController : MonoBehaviour
{
    private BossState BossState;
    public int scoreValue = 500; //Score the player receives upon killing the boss
    private float health = 250;
    public float damage;
    private bool spottedPlayer = false;
    
    //Fields for animator
    private Animator animController;
    private int speedHashId;
    private int attackHashId;
    private int deathHashId;
    
    //Used for managing the AI and movement.
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToStartAttackingTarget;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        //Creates hashes of parameters used by the animator and sets up the animator
        speedHashId = Animator.StringToHash("speed");
        attackHashId = Animator.StringToHash("attack");
        deathHashId = Animator.StringToHash("death");
        animController = GetComponent<Animator>();

        //Sets up variables for managing AI
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = GameObject.Find("Player").transform;
        distanceToStartAttackingTarget = navMeshAgent.stoppingDistance;
        navMeshAgent.isStopped = true;
    }

    void Update()
    {
        /**
         * Checks distance from target:
         *  If distance to target less than 75.0f then spot the player.
         *  If player spotted start chasing the player
         *  If player not spotted, stay idle.
         */
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        navMeshAgent.SetDestination(target.position);
        if (distanceToTarget <= 75.0f)
        {
            spottedPlayer = true;
        }
        if (spottedPlayer == true && health > 0)
        {
            Chase();
        }
        else
        {
            Idle();
        }
    }

    void Chase()
    {
        /**
         * Causes the boss to chase the player until it is close enough to attack, and then attacks the player
         */
        
        //Attack if close to player
        if (navMeshAgent.remainingDistance <= distanceToStartAttackingTarget)
        {
            if(navMeshAgent.speed != 0.0f)
            {
                navMeshAgent.speed = 0.0f;
                animController.SetFloat(speedHashId, 0.0f);
                animController.SetBool(attackHashId, true);
                navMeshAgent.isStopped = true;
            }
        }

        //Continue chasing if not close to player
        else
        {
            if (navMeshAgent.speed != 30.0f)
            {
                navMeshAgent.speed = 20.0f;
                animController.SetFloat(speedHashId, 20.0f);
                animController.SetBool(attackHashId, false);
            }
            navMeshAgent.isStopped = false;
        }
    }

    void Idle()
    {
        /**
         * The boss remains stood still.
         */
        animController.SetFloat(speedHashId, 0.0f);
        navMeshAgent.speed = 0.0f;
        navMeshAgent.isStopped = true;
    }

    public bool onHit()
    {
        /**
         * Causes the boss to lose 50 hitpoints.
         * If boss has 0 or less hitpoints then the PlayDeath coroutine is started.
         */
        health = health - 50;
        if (health <= 0)
        {
            spottedPlayer = false;
            navMeshAgent.isStopped = true;
            this.tag = null;
            StartCoroutine(PlayDeath());
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator PlayDeath()
    {
        /**
         * Plays the boss's audio death sound and causes the death animation to play. Boss object is then destroyed.
         */
        animController.SetTrigger(deathHashId);
        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
