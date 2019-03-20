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
        navMeshAgent.stoppingDistance = 7.5f;
        if (waypoints.Length == 0)
            Debug.LogError("Error: list of waypoints is empty.");
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= distanceToStartChasingTarget && angle < 180.0f && target.tag == "Player")
        {
            state = AgentState.Chasing;
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
        if (navMeshAgent.speed != 10.0f)
        {
            navMeshAgent.speed = 10.0f;
        }
        animController.SetFloat(speedHashId, 10.0f);
        navMeshAgent.SetDestination(target.position);
        if (navMeshAgent.remainingDistance <= distanceToStartAttackingTarget)
        {
            animController.SetTrigger(attackHashId);
            Debug.Log("ATTACK");
            //animController.ResetTrigger(attackHashId);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Idle();
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.isStopped = false;
        }
        if (navMeshAgent.remainingDistance > distanceToStartChasingTarget) {
            state = AgentState.Idle;
        }
    }

    void Idle()
    {
        animController.SetFloat(speedHashId, 0.0f);
        navMeshAgent.speed = 0.0f;
        navMeshAgent.isStopped = true;
    }

    void Patrol()
    {
        if (navMeshAgent.speed != 5.0f)
        {
            navMeshAgent.speed = 5.0f;
        }
        animController.SetFloat(speedHashId, 5.0f);
        navMeshAgent.isStopped = false;
        if (navMeshAgent.remainingDistance <= distanceToStartHeadingToNextWaypoint)
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
