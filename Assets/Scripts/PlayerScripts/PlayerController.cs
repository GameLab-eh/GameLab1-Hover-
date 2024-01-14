using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //for designer
    [Header("Input")]
    [SerializeField] KeyCode keyJump1 = KeyCode.J;
    [SerializeField] KeyCode keyJump2 = KeyCode.Alpha1;
    [SerializeField] KeyCode keyWall1 = KeyCode.K;
    [SerializeField] KeyCode keyWall2 = KeyCode.Alpha2;
    [SerializeField] KeyCode keyInvisibility1 = KeyCode.L;
    [SerializeField] KeyCode keyInvisibility2 = KeyCode.Alpha3;

    //checks
    private bool _isAbleToMove = true;
    private bool _isAlive = true;
    private bool _isGrounded => Physics.Raycast(transform.position, -transform.up, _playerHeight, groundLayer);
    private bool _isInvisible = false;

    //general variables
    private bool isPlayerAbleToMove = true;
    private int _playerScore;

    //basic movement variables
    private float _horizontal;
    private float _vertical;

    

    [Header("Physics")]
    

    [SerializeField, Min (0)] float _movementSpeed;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _rotationDecayRate = 5.0f;
    private float _currentRotationSpeed = 0.0f;
    private float _normalMaxSpeed; //used for store MaxSpeedVariable to restore to default when changed (currently not used)




    //for buff and malus variables
    //jump
    [SerializeField] float _jumpPower;
    [SerializeField] float _jumpStack = 10;
    [SerializeField] float _invisibilityStack = 10;
    [SerializeField,Min(0)] float _playerHeight;
    [field: SerializeField] public Rigidbody Rb { get; private set; }

    //step on stairs variables
    [Header("Stairs variable")]
    [SerializeField] Transform _stairsUpperPoint;
    [SerializeField] Transform _stairsLowerPoint;
    [SerializeField] float _stairsJumps = 0.1f;
    [SerializeField] LayerMask groundLayer;





    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
       
    }

    private void Update()
    {
        if (_isAlive)
        {
            if (_isAbleToMove)
            {
                _horizontal = Input.GetAxisRaw("Rotation");
                _vertical = Input.GetAxisRaw("Move");
                rotate();
            }
            if (Input.GetKeyDown(keyJump1) || Input.GetKeyDown(keyJump2))
            {
                Jump();
            }
            if (Input.GetKeyDown(keyWall1) || Input.GetKeyDown(keyWall2))
            {
                Wall();
            }
            if (Input.GetKeyDown(keyInvisibility1) || Input.GetKeyDown(keyInvisibility2))
            {
                Debug.Log(_isInvisible);
                StartCoroutine(Invisibility());
            }
        }
    }


    void FixedUpdate()
    {
        Move();
        StairsClimb();
    }

    private void rotate()
    {

        if (_horizontal != 0)
        {
            _currentRotationSpeed = _horizontal * _rotationSpeed;
            float angle = _currentRotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, angle);
        }
        else if (_currentRotationSpeed != 0)
        {
            //gradualy reduce rotation speed over time
            float decay = _rotationDecayRate * Time.deltaTime;
            _currentRotationSpeed = Mathf.Lerp(_currentRotationSpeed, 0, decay);
            transform.Rotate(Vector3.up, _currentRotationSpeed * Time.deltaTime);
        }

    }

    private void Move()
    {

        if (Rb.velocity.magnitude < _maxSpeed)

        {
            Vector3 move = transform.forward * _vertical * _movementSpeed;
            Rb.AddForce(move, ForceMode.Force);
        }
    }




    private void Jump()
    {
        if(_isGrounded && _jumpStack > 0)
        {
            Rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
            _jumpStack--;
        }
    }

    private void Wall()
    {
        Debug.Log("ENTRA?!?!?!?");
    }

    public IEnumerator Invisibility()
    {
        if(!_isInvisible)
        {
            _invisibilityStack--;
            _isInvisible = true;
            Debug.Log("hey sono invisibile");
            yield return new WaitForSeconds(5f);
            _isInvisible = false;
            Debug.Log("hey non sono più invisibile");
        }
    }
    
    private void StairsClimb()
    {
        Debug.DrawRay(_stairsLowerPoint.position, transform.forward * 0.1f);
        Debug.DrawRay(_stairsUpperPoint.position, transform.forward * 0.5f);
        RaycastHit lowerHit;
        if(Physics.Raycast(_stairsLowerPoint.position, transform.forward, out lowerHit, 0.2f, groundLayer))
        {
            
            RaycastHit upperHit;
            if (!Physics.Raycast(_stairsUpperPoint.position, transform.forward, out upperHit, 0.5f, groundLayer))
            {
                Rb.position += new Vector3(0, _stairsJumps, 0.5f);
                
            }
        }
    }

    


}
