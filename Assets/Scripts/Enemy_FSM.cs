using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy_FSM : MonoBehaviour
{
    public enum ENEMY_STATE { Spawned, Chase, Follow, Kill   };
    [SerializeField]
    private ENEMY_STATE currentState;
    public ENEMY_STATE CurrentState
    {
        get
        {
            return currentState;

        }
        set
        {

            currentState = value;
            StopAllCoroutines();
            switch (currentState)
            {
                case ENEMY_STATE.Spawned:
                    StartCoroutine(EnemyChase());
                    break;
                case ENEMY_STATE.Chase:
                    StartCoroutine(EnemyChase());
                    break;
                case ENEMY_STATE.Follow:
                    StartCoroutine(EnemyChase());
                    break;
                case ENEMY_STATE.Kill:
                    StartCoroutine(EnemyAttack());
                    break;
            }

        }
    }

    private CheckMyVision checkMyVision;
    private NavMeshAgent agent = null;
    public Transform playerTransform = null;
    private Transform patrolDestination = null;
    private Health playerHealth = null;
    public float maxDamage = 10f;
    private void Awake()
    {
        checkMyVision = GetComponent<CheckMyVision>();
        agent = GetComponent<NavMeshAgent>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerTransform = playerHealth.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] destinations = GameObject.FindGameObjectsWithTag("Dest");
        patrolDestination = destinations[Random.Range(0, destinations.Length)].GetComponent<Transform>();
        CurrentState = ENEMY_STATE.Spawned;
    }

    public IEnumerator EnemyPatrol()
    {
        while (currentState == ENEMY_STATE.Spawned)
        {
            checkMyVision.sensitity = CheckMyVision.enmSensitivity.HIGH;
            agent.isStopped = false;
            agent.SetDestination(patrolDestination.position);
            while (agent.pathPending)
            {
                yield return null;
            }

            if (checkMyVision.targetInSight)
            {
                agent.isStopped = true;
                CurrentState = ENEMY_STATE.Chase;
                yield break;
            }
            yield return null;
        }

    }
    public IEnumerator EnemyChase()
    {
        while (currentState == ENEMY_STATE.Chase)
        {
            checkMyVision.sensitity = CheckMyVision.enmSensitivity.LOW;
            agent.isStopped = false;
            agent.SetDestination(checkMyVision.lastknownSighting);
            while (agent.pathPending)
            {
                yield return null;
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                if (!checkMyVision.targetInSight)
                {
                    CurrentState = ENEMY_STATE.Spawned;
                }
                else
                {

                    CurrentState = ENEMY_STATE.Kill;
                }
                yield break;
            }
            yield return null;
        }
        yield break;
    }
    public IEnumerator EnemyAttack()
    {
        while (currentState == ENEMY_STATE.Kill)
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
            while (agent.pathPending)
                yield return null;
            if (agent.remainingDistance > agent.stoppingDistance)
            { 
                CurrentState = ENEMY_STATE.Chase;
                yield break;
            }
            else
            {
                playerHealth.HealthPoints -= maxDamage * Time.deltaTime;
            }
            yield return null; 
        }
        yield break;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
