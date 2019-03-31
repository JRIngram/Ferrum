using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public struct BossState
{
    public Vector3 position;
    public Quaternion rotation;
    public float health;
    public bool spottedPlayer;

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
    public int scoreValue = 500;
    private float health = 250;
    public float damage;
    private NavMeshAgent navMeshAgent;
    private Animator animController;
    private int speedHashId;
    private int attackHashId;
    private int deathHashId;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToStartAttackingTarget;
    private BossState BossState;
    private bool spottedPlayer = false;

    void Awake()
    {
        speedHashId = Animator.StringToHash("speed");
        attackHashId = Animator.StringToHash("attack");
        deathHashId = Animator.StringToHash("death");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
        this.target = GameObject.Find("Player").transform;
        distanceToStartAttackingTarget = navMeshAgent.stoppingDistance;
        navMeshAgent.isStopped = true;
    }

    void Update()
    {
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
                navMeshAgent.speed = 30.0f;
                animController.SetFloat(speedHashId, 30.0f);
                animController.SetBool(attackHashId, false);
            }
            navMeshAgent.isStopped = false;
        }
    }

    void Idle()
    {
        animController.SetFloat(speedHashId, 0.0f);
        navMeshAgent.speed = 0.0f;
        navMeshAgent.isStopped = true;
    }

    public bool onHit()
    {
        health = health - 50;
        if (health == 0)
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
        //Waits two frames before reactivating script so that the character is loaded into the correct position.
        Debug.Log("Am dying");
        animController.SetTrigger(deathHashId);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
