using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class AiController : MonoBehaviour
{
    //generals and checks
     
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    
    
    
    //patrol variables
    [SerializeField] private float _walkRadius;
    private bool _isWalkSet;
    [SerializeField, Tooltip("max range the enemy can chose a point to move")] private float walkRange;

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _rayStartingPoint;

    [Header("checks and states of the patrolling/chasing")] 
    [SerializeField] private float _visualAngle = 45f;
    private int _rayCountNumber = 5;
    private float _raySpacing;
    [SerializeField] private float _chaseRange;
    [SerializeField] private float _visualRange;

    private void Awake()
    {
        _raySpacing = _visualAngle / _rayCountNumber;
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
        
        

        // if (!_isWalkSet)
        // {
        //     Vector3 randomPoint = Random.insideUnitSphere * _walkRadius;
        //     NavMeshHit hit;
        //     if (NavMesh.SamplePosition(randomPoint, out hit, _walkRadius, NavMesh.AllAreas))
        //     {
        //         NavMeshPath path = new NavMeshPath();
        //         if (_navMeshAgent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
        //         {
        //             _navMeshAgent.SetDestination(hit.position);
        //             _isWalkSet = true;
        //         }
        //         else
        //         {
        //             WalkPointSet();
        //         }
        //     }
        //     else
        //     {
        //         WalkPointSet();
        //     }
        //     
        // }
        // if (Vector3.Distance(_navMeshAgent.destination,transform.position)<0.1f)
        // {
        //     _isWalkSet = false;
        // }
        
    }

    private Vector3 GetRandomPoint() 
    {
        Vector3 randomPoint = Random.insideUnitSphere * _walkRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, _walkRadius, NavMesh.AllAreas);
        if (IsPointReachable(hit.position))
        {
            return hit.position;
        }
        return Vector3.zero; //rimuoverlo creando un ciclo che esce quando IsPointReachable ritorna true. (Farò riposare i miei neuorini per ora)
    }
    private void GoToPosition(Vector3 pointToGo)
    {
        _navMeshAgent.SetDestination(pointToGo);
    }
    private bool IsPointReachable(Vector3 pointToVerify)
    {
        NavMeshPath path = new NavMeshPath();
        return (_navMeshAgent.CalculatePath(pointToVerify, path) && path.status == NavMeshPathStatus.PathComplete);
    }
    
    
    
    private void WalkPointSet()
    {
        Vector3 randomPoint = Random.insideUnitSphere * _walkRadius;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, _walkRadius, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            if (_navMeshAgent.CalculatePath(hit.position, path))
            {
                _navMeshAgent.SetDestination(hit.position);
            }
            else
            {
                WalkPointSet();
            }
        }
        else
        {
            WalkPointSet();
        }

        
            

    }

    private void Chase()
    {
        _navMeshAgent.SetDestination(_player.position);
    }
    private bool IsPlayerInRange()
    {
        for (int i = 0; i < _rayCountNumber; i++)
        {
            float angle = -_visualAngle / 2 + i * _raySpacing;
            
            float angleInRadians = angle * Mathf.Deg2Rad;
            
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 rayDirection = transform.rotation * rotation * transform.forward;
            
            Debug.DrawRay(_rayStartingPoint.position,transform.TransformDirection(rayDirection) * _visualRange,Color.green,_playerLayer);
            if (Physics.Raycast(_rayStartingPoint.position, transform.TransformDirection(rayDirection), _visualRange, _playerLayer))
            {
                return true;
            }
        }
        return false;
    }
}
