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


    [SerializeField, Min(0)] float _movementSpeed;
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
    float _invisibilityDuration;

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

    //UI
    public delegate void Speed(float score);
    public static event Speed PlayerSpeed = null;

    public delegate void PowerUpInfoInt(int t, int value);
    public static event PowerUpInfoInt Stack = null; //jump
    public static event PowerUpInfoInt StackUse = null; //wall & invisibility

    public delegate void PowerUpInfoBool(bool value);
    public static event PowerUpInfoBool Stoplight = null;

    public delegate void PowerUpInfo();
    public static event PowerUpInfo Shield = null; // shield


    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        _normalMaxSpeed = _maxSpeed;
        _invisibilityDuration = GameManager.Instance.GetInvisibilityDuration();
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
                Stack?.Invoke(0, (int)_jumpStack);
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

        //UI
        Vector3 _tmpVelocity = Rb.velocity;
        PlayerSpeed?.Invoke(Mathf.Clamp((Mathf.Round(((_tmpVelocity.magnitude / _maxSpeed) * 75f) * 10f) / 10f), 0f, 100f));

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
        if (_isGrounded && _jumpStack > 0)
        {
            Rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
            _jumpStack--;
        }
    }

    private void Wall()
    {
        _wallStack--;
        StackUse?.Invoke(1, (int)_wallStack);
        GameObject wall = ObjectPooler.SharedInstance.GetPooledObject();
        wall.SetActive(true);
        wall.transform.position = _wallPoint.position;
        wall.transform.rotation = _wallPoint.transform.rotation;
        _wallStack--;
    }

    public IEnumerator Invisibility()
    {
        if (!_isInvisible)
        {
            _invisibilityStack--;
            StackUse?.Invoke(2, (int)_invisibilityStack);
            _isInvisible = true;
            Debug.Log("hey sono invisibile");
            yield return new WaitForSeconds(_invisibilityDuration);
            _isInvisible = false;
            Debug.Log("hey non sono più invisibile");
        }
    }

    private void StairsClimb()
    {

        RaycastHit lowerHit;
        RaycastHit lowerHit45;
        RaycastHit lowerHitOther45;
        if (Physics.Raycast(_stairsLowerPoint.position, transform.forward, out lowerHit, 0.3f, _groundLayer))
        {
            RaycastHit upperHit;
            if (!Physics.Raycast(_stairsUpperPoint.position, transform.forward, out upperHit, 0.5f, _groundLayer))
            {
                Rb.position += new Vector3(0, _stairsJumps, 0f);
            }
        }
        else if (Physics.Raycast(_stairsLowerPoint.position, transform.TransformDirection(1.5f, 0, 1), out lowerHit45, 0.3f, _groundLayer))
        {
            RaycastHit upperHit45;
            if (!Physics.Raycast(_stairsUpperPoint.position, transform.TransformDirection(1.5f, 0, 1), out upperHit45, 0.5f, _groundLayer))
            {
                Rb.position += new Vector3(0, _stairsJumps, 0f);
            }
        }
        else if (Physics.Raycast(_stairsLowerPoint.position, transform.TransformDirection(-1.5f, 0, 1), out lowerHitOther45, 0.3f, _groundLayer))
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
        if (layer == 6)
        {
            layer = Random.Range(7, 13);
        }
        switch (layer)
        {
            case 7:
                _jumpStack++;
                Stack?.Invoke(0, (int)_jumpStack);
                break;
            case 8:
                _invisibilityStack++;
                Stack?.Invoke(2, (int)_invisibilityStack);
                break;
            case 9:
                _wallStack++;
                Stack?.Invoke(1, (int)_wallStack);
                break;
            case 10:
                _shieldStack++;
                Shield?.Invoke();
                break;
            case 11:
                //red
                _maxSpeed = _maxSpeed - _speedChanger;
                Invoke("NormalizeMaxSpeedVar", _timeSpeedChanger);
                Stoplight?.Invoke(false);
                break;
            case 12:
                //green
                _maxSpeed = _maxSpeed + _speedChanger;
                Invoke("NormalizeMaxSpeedVar", _timeSpeedChanger);
                Stoplight?.Invoke(true);
                break;
        }

        if (collision.gameObject.CompareTag("Flag")) return; //Exception for Flag

        Destroy(collision.gameObject);
    }
    private void NormalizeMaxSpeedVar()
    {
        _maxSpeed = _normalMaxSpeed;
    }

}
