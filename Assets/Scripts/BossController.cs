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
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToStartAttackingTarget;
    private BossState BossState;
    private bool spottedPlayer = false;

    void Awake()
    {
        speedHashId = Animator.StringToHash("speed");
        attackHashId = Animator.StringToHash("attack");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
        this.target = GameObject.Find("Player").transform;
        distanceToStartAttackingTarget = navMeshAgent.stoppingDistance;
    }

    void Update()
    {
        if (spottedPlayer == true)
        {
            Chase();
        }
        else
        {
            Debug.Log("AM IDLE");
            Idle();
        }
    }

    void Chase()
    {
        navMeshAgent.SetDestination(target.position);
        Debug.Log(target.position);
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
            Debug.Log("FUCKING MOVE IT");
            if (navMeshAgent.speed != 20.0f)
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
        animController.SetFloat(speedHashId, 0.0f);
        navMeshAgent.speed = 0.0f;
        navMeshAgent.isStopped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Debug.Log("entered");
            spottedPlayer = true;
        }
    }
}
