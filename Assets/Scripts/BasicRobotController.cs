using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BasicRobotController : MonoBehaviour
{
    public int scoreValue = 100;
    private int health = 100;
    public AgentState state;
    public Transform[] waypoints;
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

    public enum AgentState
    {
        Idle = 0,
        Patrolling,
        Chasing
    }

    void Awake()
    {
        speedHashId = Animator.StringToHash("walkingSpeed");
        attackHashId = Animator.StringToHash("attack");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animController = GetComponent<Animator>();
        next_waypoint = 0;
        navMeshAgent.stoppingDistance = 2.0f;
        if (waypoints.Length == 0)
            Debug.LogError("Error: list of waypoints is empty.");
    }

    void Update()
    {
        navMeshAgent.SetDestination(target.position);
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        if (navMeshAgent.remainingDistance <= distanceToStartChasingTarget && angle < 90.0f)
            Chase();
        else if (state == AgentState.Idle)
            Idle();
        else if (state == AgentState.Patrolling)
            Patrol();
    }

    void Chase()
//TODO NEED TO MAKE CHASING A BIT MORE INTELLIGENT.
    {
        if (navMeshAgent.speed != 5.0f)
        {
            navMeshAgent.speed = 7.5f;

        }
        animController.SetFloat(speedHashId, 7.5f);
        navMeshAgent.SetDestination(target.position);
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        navMeshAgent.isStopped = false;
        if (navMeshAgent.remainingDistance <= distanceToStartAttackingTarget)
        {
            animController.SetTrigger(attackHashId);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Idle();
        }
    }

    void Idle()
    {
        animController.SetFloat(speedHashId, 0.0f);
        navMeshAgent.isStopped = true;
    }

    void Patrol()
    {
        if (navMeshAgent.speed != 3.5f)
        {
            navMeshAgent.speed = 3.5f;
        }
        animController.SetFloat(speedHashId, 3.5f);
        navMeshAgent.isStopped = false;
        if (navMeshAgent.remainingDistance < distanceToStartHeadingToNextWaypoint)
        {
            Debug.Log(waypoints[next_waypoint]);
            next_waypoint = (next_waypoint + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[next_waypoint].position);
        }
    }


    public bool onHit() {
        health = health - 50;
        if (health == 0)
        {
            Destroy(gameObject);
            return true;
        }
        else {
            return false;
        }
    }
}
