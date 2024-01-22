using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class AiController : MonoBehaviour
{
    //generals and checks

    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _player;
    [SerializeField] private float _knockBackForce;


    //patrol variables
    [SerializeField] private float _walkRadius;
    private bool _isWalkSet;

    [SerializeField, Tooltip("max range the enemy can chose a point to move")]
    private float walkRange;

    [SerializeField] private LayerMask _playerLayer;


    [Header("checks and states of the patrolling/chasing")]
    [SerializeField, Tooltip("always set it to half the angle u want (if u want 45° set it to 22.5)")]
    private float _visualAngle = 22.5f;


    [SerializeField] private float _visualRange;

    private void Awake()
    {

        _player = GameObject.Find("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (IsPlayerInRange())
        {
            Chase();
        }
        else
        {
            Patroling();
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

        do
        {
            randomPoint = Random.onUnitSphere * _walkRadius;
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

    private void Chase()
    {
        _navMeshAgent.SetDestination(_player.position);
    }
    private bool IsPlayerInRange()
    {
        //distance between me and player
        Vector3 toPlayer = (_player.position - transform.position);

        //check maxDistance
        if (toPlayer.sqrMagnitude > _visualRange * _visualRange)
        {
            return false;
        }
        toPlayer.Normalize();

        //calculate angle, my direction / player direction
        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);

        //check if angle is inside 45° angle degree
        if (angleToPlayer <= _visualAngle)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 31)
        {
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(knockbackDirection * _knockBackForce, ForceMode.Impulse);
        }
    }
}
