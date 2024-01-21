using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AiScout : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private bool isArraySet;

    [SerializeField] private float _walkRadius;
    private bool _isWalkSet;

    [SerializeField, Tooltip("max range the enemy can chose a point to move")]
    private float walkRange;

    [Header("checks and states of the patrolling/chasing")]
    [SerializeField, Tooltip("always set it to half the angle u want (if u want 45° set it to 22.5)")]
    private float _visualAngle = 22.5f;

    [SerializeField] private float _visualRange;
    private GameObject[] _enemyFlagArray;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(FlagsToArray());
    }

    private void Update()
    {
        if (isArraySet)
        {
            if (IsFlagInRange())
            {
                GetFlag();
            }
            else
            {
                Patroling();
            }
        }
    }

    private void Patroling()
    {
        if (!_isWalkSet)
        {
            WalkPointSet();
            _isWalkSet = true;
        }

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending)
        {
            _isWalkSet = false;
        }
    }

    private Vector3 GetRandomPoint()
    {
        NavMeshHit hit;
        Vector3 randomPoint;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            randomPoint = Random.onUnitSphere * _walkRadius;
            attempts++;
            hit = new NavMeshHit();
            // Aggiungi una condizione per uscire dal ciclo dopo un numero massimo di tentativi
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Impossibile trovare una posizione valida dopo " + maxAttempts + " tentativi.");
                break;
            }
        } while (!NavMesh.SamplePosition(randomPoint, out hit, _walkRadius, NavMesh.AllAreas));

        return hit.position;
    }

    private bool IsPointReachable(Vector3 pointToVerify)
    {
        NavMeshPath path = new NavMeshPath();
        return (_navMeshAgent.CalculatePath(pointToVerify, path) && path.status == NavMeshPathStatus.PathComplete);
    }

    private void WalkPointSet()
    {
        _navMeshAgent.SetDestination(GetRandomPoint());
    }

    private void GetFlag()
    {
        _navMeshAgent.SetDestination(FindClosestFlag().transform.position);
    }

    private GameObject FindClosestFlag()
    {
        GameObject actualClosestFlag = _enemyFlagArray[0];
        float closestDistance = Vector3.Distance(transform.position, actualClosestFlag.transform.position);

        foreach (var flag in _enemyFlagArray)
        {
            float distanceToTheCheckingFlag = Vector3.Distance(transform.position, flag.transform.position);
            float distanceToTheClosestFlag = Vector3.Distance(transform.position, actualClosestFlag.transform.position);

            if (distanceToTheCheckingFlag < distanceToTheClosestFlag)
            {
                actualClosestFlag = flag;
                closestDistance = distanceToTheCheckingFlag;
            }
        }

        return actualClosestFlag;
    }

    private bool IsFlagInRange()
    {
        if (_enemyFlagArray.Length == 0)
        {
            return false;
        }

        foreach (var flag in _enemyFlagArray)
        {
            Vector3 toFlag = flag.transform.position - transform.position;

            if (toFlag.sqrMagnitude > _visualRange * _visualRange)
            {
                continue;
            }

            toFlag.Normalize();

            float angleToFlag = Vector3.Angle(transform.forward, toFlag);

            if (angleToFlag <= _visualAngle)
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator FlagsToArray()
    {
        yield return new WaitForSeconds(1f);
        _enemyFlagArray = GameObject.FindGameObjectsWithTag("Flag");
        isArraySet = true;
    }
}
    