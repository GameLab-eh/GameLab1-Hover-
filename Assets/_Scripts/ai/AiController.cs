using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class AiController : MonoBehaviour
{
    //generals and checks

    private Rigidbody rb;
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

    private bool _isPlayerHitted = false;
    [SerializeField] private float secondsBeforeChasingAgain = 1f;
    public static bool _isPlayerInvisible = false;

    [SerializeField] private float _visualRange;
    
    [SerializeField] private float rotateTowardsSpeed;
    [SerializeField] private float _boostForce;
    
    [SerializeField] private float _blockHeight;
    [SerializeField] private float _blockDuration;
    [SerializeField] private float _descentSpeed;
    [SerializeField] private float quickSandMaxDistance;
    private float _originPosition;
    private bool _isAbleToMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _player = GameObject.Find("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _originPosition = transform.position.y;
    }
    private void Update()
    {
        if (_isAbleToMove)
        {
            if (IsPlayerInRange() && !_isPlayerHitted && !_isPlayerInvisible)
            {
                Chase();
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
            rb.AddForce(knockbackDirection * _knockBackForce, ForceMode.Impulse);
            StartCoroutine(playerHitted());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 25)
        {
            StartCoroutine(Quicksand(other.gameObject));
        }
        if (other.gameObject.layer == 26)
        {
            GroundBoost(other.gameObject);
        }
    }
    private IEnumerator playerHitted()
    {
        _isPlayerHitted = true;
        yield return new WaitForSeconds(secondsBeforeChasingAgain);
        _isPlayerHitted = false;
    }
    private void GroundBoost(GameObject collObj)
    {
        Debug.Log("almeno entro");
        Vector3 targetDirection = collObj.transform.forward;
        StartCoroutine(RotateTowardsDirection(targetDirection, rotateTowardsSpeed));
        Vector3 push = collObj.transform.forward * _boostForce;
        rb.AddForce(push, ForceMode.Impulse);
    }
    private IEnumerator RotateTowardsDirection(Vector3 targetDirection, float rotateTowardsSpeed)
    {
        float angleToRotate = Vector3.Angle(transform.forward, targetDirection);

        while (angleToRotate > 0.1f)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotateTowardsSpeed, 0.0f);
            angleToRotate = Vector3.Angle(transform.forward, targetDirection);

            yield return null;
        }
    }
    private IEnumerator Quicksand(GameObject collObj)
    {
        float startTime = Time.time;
        float startAltitude = transform.transform.position.y;

        while (transform.position.y > _blockHeight)
        {
            if (IsOutQuicksand(collObj))
            {
                transform.position = new Vector3(transform.position.x, _originPosition, transform.position.z);
                
                yield break;                        //float closestDistance = Vector3.Distance(transform.position, actualClosestFlag.transform.position);
            }
            float progress = (Time.time - startTime) / _blockDuration;
            float interpolation = Mathf.Pow(progress, 2);
            float newY = Mathf.Lerp(startAltitude, _blockHeight, interpolation * _descentSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            yield return null;
        }
        
        _isAbleToMove = false;
        _navMeshAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(_blockDuration);

        _isAbleToMove = true;
        transform.position = new Vector3(transform.position.x, _originPosition, transform.position.z);
    }
    private bool IsOutQuicksand(GameObject collObj)
    {
        float verticalDistance = Mathf.Abs(transform.position.y - collObj.transform.position.y);
        return verticalDistance > quickSandMaxDistance;
    }
}
