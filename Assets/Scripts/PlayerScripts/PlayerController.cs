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
    private bool _isGrounded => Physics.Raycast(transform.position, -transform.up, _playerHeight, _groundLayer);
    private bool _isInvisible = false;

    //general variables
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
    [SerializeField, Min(0)] float _playerHeight;
    [field: SerializeField] public Rigidbody Rb { get; private set; }


    //for buff and malus variables
    //jump
    [Header("Buff and Debuff")]
    [SerializeField] float _jumpPower;
    [SerializeField] float _jumpStack = 10;

    //invisibility
    [SerializeField] float _invisibilityStack = 10;


    //wall
    [SerializeField] float _wallStack = 10;
    [SerializeField] Transform _wallPoint;

    //shield
    [SerializeField] float _shieldStack = 10;

    //Green and Red light
    [SerializeField] float _speedChanger;
    [SerializeField] float _timeSpeedChanger;

    //step on stairs variables
    [Header("Stairs variable")]
    [SerializeField] Transform _stairsUpperPoint;
    [SerializeField] Transform _stairsLowerPoint;
    [SerializeField] float _stairsJumps = 0.1f;
    [SerializeField] LayerMask _groundLayer;
    




    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _normalMaxSpeed = _maxSpeed;
       
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
        GameObject wall = ObjectPooler.SharedInstance.GetPooledObject();
        wall.SetActive(true);
        wall.transform.position = _wallPoint.position;
        wall.transform.rotation = _wallPoint.transform.rotation;
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
        RaycastHit lowerHit;
        RaycastHit lowerHit45;
        RaycastHit lowerHitOther45;
        if (Physics.Raycast(_stairsLowerPoint.position, transform.forward, out lowerHit, 0.2f, _groundLayer))
        {
            RaycastHit upperHit;
            if (!Physics.Raycast(_stairsUpperPoint.position, transform.forward, out upperHit, 0.5f, _groundLayer))
            {
                Rb.position += new Vector3(0, _stairsJumps, 0f);
            }
        }
        else if (Physics.Raycast(_stairsLowerPoint.position, transform.TransformDirection(1.5f, 0, 1), out lowerHit45, 0.2f, _groundLayer))
        {
            RaycastHit upperHit45;
            if (!Physics.Raycast(_stairsUpperPoint.position, transform.TransformDirection(1.5f, 0, 1), out upperHit45, 0.5f, _groundLayer))
            {
                Rb.position += new Vector3(0, _stairsJumps, 0f);
            }
        }
        else if (Physics.Raycast(_stairsLowerPoint.position, transform.TransformDirection(-1.5f, 0, 1), out lowerHitOther45, 0.2f, _groundLayer))
        {
            RaycastHit upperHitOther45;
            if (!Physics.Raycast(_stairsUpperPoint.position, transform.TransformDirection(-1.5f, 0, 1), out upperHitOther45, 0.5f, _groundLayer))
            {
                Rb.position += new Vector3(0, _stairsJumps, 0f);
            }
        }



    }
    private void OnTriggerEnter(Collider collision)
    {
        int layer = collision.gameObject.layer;

        switch(layer)
        {
            case 6:
                Debug.Log("random");
                break;
            case 7:
                _jumpStack++;
                break;
            case 8:
                _invisibilityStack++;
                break;
            case 9:
                _wallStack++;
                break;
            case 10:
                _shieldStack++;
                break;
            case 11:
                _maxSpeed = _maxSpeed - _speedChanger;
                Invoke("NormalizeMaxSpeedVar", _timeSpeedChanger);
                break;
            case 12:
                _maxSpeed = _maxSpeed + _speedChanger;
                Invoke("NormalizeMaxSpeedVar",_timeSpeedChanger);
                break;
        }
    }
    private void NormalizeMaxSpeedVar()
    {
        _maxSpeed = _normalMaxSpeed;
    }





}
