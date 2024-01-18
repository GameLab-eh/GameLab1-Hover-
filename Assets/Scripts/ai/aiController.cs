using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiController : MonoBehaviour
{
    //generals and checks
    
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Transform _rayStartingPoint;

    [Header("checks and states of the patrolling/chasing")] 
    [SerializeField] private float _visualAngle = 45f;
    private int _rayCountNumber = 5;
    private float _raySpacing;
    [SerializeField] private float _chaseRange;
    [SerializeField] private float _visualRange;

    private void Update()
    {
        _IsPlayerInRange();
    }

    private void Awake()
    {
        _raySpacing = _visualAngle / _rayCountNumber;
        //_player = GameObject.Find("player").transform;
        //_navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Patroling()
    {
        
    }

    private void Chase()
    {
        
    }
    [SerializeField] private bool _IsPlayerInRange()
    {
        for (int i = 0; i < _rayCountNumber; i++)
        {
            float angle = -_visualAngle / 2 + i * _raySpacing;
            
            float angleInRadians = angle * Mathf.Deg2Rad;
            
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 rayDirection = transform.rotation * rotation * transform.forward;
            
            Debug.DrawRay(_rayStartingPoint.position,transform.TransformDirection(rayDirection),Color.green,_visualAngle);
            if (Physics.Raycast(_rayStartingPoint.position, transform.TransformDirection(rayDirection), _visualAngle, _playerLayer))
            {
                return true;
            }
        }
        return false;
    }
}
