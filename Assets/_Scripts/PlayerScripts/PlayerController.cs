using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

    public List<Transform> spawnPoints = new List<Transform>();
    [Header("Input")]
    KeyCode keyJump1;
    KeyCode keyWall1;
    KeyCode keyInvisibility1;
    
    //checks
    private bool _isAbleToMove = true;
    private bool _isAlive = true;
    private bool _isGrounded;
    private bool _isInvisible;
    private bool _isModern;
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
    private float _currentRotationSpeed;
    private float _normalMaxSpeed;
    [field: SerializeField] public Rigidbody Rb { get; private set; }
    [SerializeField] private float _knockBackForce;

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
    private bool _isShielded;
    [SerializeField] private float _shieldTimer;

    //Green and Red light
    [SerializeField] float _speedChanger;
    [SerializeField] float _timeSpeedChanger;

    //step on stairs variables
    [Header("Stairs variable")]
    [SerializeField] Transform _stairsUpperPoint;
    [SerializeField] Transform _stairsLowerPoint;
    [SerializeField] float _stairsJumps = 0.1f;
    [SerializeField] LayerMask _groundLayer;
    
    //ground variables
    [Header("Ground power ups variables")]
    [SerializeField] private float rotateTowardsSpeed;
    [SerializeField] private float _boostForce;
    private float _keepBlockingIndex;

    [Header("QuickSand")] 
    [SerializeField] private float _blockHeight;
    [SerializeField] private float _blockDuration;
    [SerializeField] private float _descentSpeed;
    [SerializeField] private float quickSandMaxDistance;
    private GameObject _camerasGO;
    private float _camerasOriginPosition;

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
    public static event PowerUpInfo EraseMap = null; //Erase Map


    private void Awake()
    {    
        GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");
        foreach (var spawnPointObject in spawnPointObjects)
        {
            spawnPoints.Add(spawnPointObject.transform);
        }
        Rb = GetComponent<Rigidbody>();
        _normalMaxSpeed = _maxSpeed;
        _camerasGO = GameObject.Find("PlayerCameras");
        _camerasOriginPosition = _camerasGO.transform.position.y;
    }

    private void Start()
    {
        _invisibilityDuration = GameManager.Instance.GetInvisibilityDuration();
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        transform.position = randomSpawnPoint.position;
    }

    private void Update()
    {
        _isModern = GameManager.Instance.GetInputSystem();
        
        if (_isModern)
        {
            keyJump1 = KeyCode.J;
            keyWall1 = KeyCode.K;
            keyInvisibility1 = KeyCode.L;
        }
        else
        {
            keyJump1 = KeyCode.A;
            keyWall1 = KeyCode.S;
            keyInvisibility1 = KeyCode.D;
        }

        if (_isAlive && _isAbleToMove)
        {
            if (!_isModern)
            {
                _horizontal = Input.GetAxisRaw("RotationLegacy");
                _vertical = Input.GetAxisRaw("MoveLegacy");
                rotate();
            }
            else
            {
                _horizontal = Input.GetAxisRaw("RotationModern");
                _vertical = Input.GetAxisRaw("MoveModern");
                rotate();
            }
            if (Input.GetKeyDown(keyJump1))
            {
                Jump();
                Stack?.Invoke(0, (int)_jumpStack);
            }
            if (Input.GetKeyDown(keyWall1))
            {
                Wall();
            }
            if (Input.GetKeyDown(keyInvisibility1))
            {
                Debug.Log(_isInvisible);
                StartCoroutine(Invisibility());
            }
        }

        //UI
        Vector3 _tmpVelocity = Rb.velocity;
        PlayerSpeed?.Invoke(Mathf.Clamp((Mathf.Round(((_tmpVelocity.magnitude / _normalMaxSpeed) * 75f) * 10f) / 10f), 0f, 100f));

    }


    void FixedUpdate()
    {
        if (_isAbleToMove)
        {
            Move();
        }
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
            float direction = _movementSpeed * _vertical;
            Vector3 move = transform.forward * direction;
            Rb.AddForce(move, ForceMode.Force);
        }
    }

    private void Jump()
    {
        if (_isGrounded && _jumpStack > 0)
        {
            GameManager.Instance.AudioManager.PlayEffect("jump");
            _isGrounded = false;
            Rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
            _jumpStack--;
        }
    }

    private void Wall()
    {
        if (_wallStack > 0)
        {
            GameManager.Instance.AudioManager.PlayEffect("wall");
            _wallStack--;
            StackUse?.Invoke(1, (int)_wallStack);
            GameObject wall = ObjectPooler.SharedInstance.GetPooledObject();
            wall.SetActive(true);
            wall.transform.position = _wallPoint.position;
            wall.transform.rotation = _wallPoint.transform.rotation;
        }

    }

    public IEnumerator Invisibility()
    {
        if (!_isInvisible && _invisibilityStack > 0)
        {
            GameManager.Instance.AudioManager.PlayEffect("invisibility");
            _invisibilityStack--;
            StackUse?.Invoke(2, (int)_invisibilityStack);
            AiController._isPlayerInvisible = true;
            _isInvisible = true;
            Debug.Log("hey sono invisibile");
            yield return new WaitForSeconds(_invisibilityDuration);
            AiController._isPlayerInvisible = false;
            _isInvisible = false;
            Debug.Log("hey non sono più invisibile");
        }
    }

    private void StairsClimb()
    {
        Vector3 rayDirection = transform.forward;
        float stairsMovement = 0.5f;
        RaycastHit lowerHit;
        // RaycastHit lowerHit45;
        // RaycastHit lowerHitOther45;
        if (_vertical < 0)
        {
            rayDirection = -transform.forward;
            stairsMovement = -0.5f;
        }
        
        
        if (Physics.Raycast(_stairsLowerPoint.position, rayDirection, out lowerHit, 0.7f, _groundLayer))
        {
            RaycastHit upperHit;
            if (!Physics.Raycast(_stairsUpperPoint.position, rayDirection, out upperHit, 1f, _groundLayer))
            {
                Rb.position += new Vector3(0, _stairsJumps, stairsMovement);
            }
        }
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 3)
        {
            _isGrounded = true;
        }
        if (other.gameObject.layer == 30)
        {
            Vector3 knockbackDirection = (transform.position - other.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(knockbackDirection * _knockBackForce, ForceMode.Impulse);
        }
    
        if (other.gameObject.CompareTag("Wood")) GameManager.Instance.AudioManager.PlayEffect("woodCrash");
        else GameManager.Instance.AudioManager.PlayEffect("crash");
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
                _isShielded = true;
                StartCoroutine(ShieldRemover());
                Shield?.Invoke();
                GameManager.Instance.AudioManager.PlayEffect("shield");
                break;
            case 11:
                //red
                _maxSpeed -= _speedChanger;
                Invoke(nameof(NormalizeMaxSpeedVar), _timeSpeedChanger);
                Stoplight?.Invoke(false);
                break;
            case 12:
                //green
                _maxSpeed += _speedChanger;
                Invoke(nameof(NormalizeMaxSpeedVar), _timeSpeedChanger);
                Stoplight?.Invoke(true);
                break;
            case 13: //EraseMap
                EraseMap?.Invoke();
                break;
            case 25:
                if (!_isShielded && _isAbleToMove)
                {
                    Debug.Log("Dovrei scendere, addio");
                    StartCoroutine(Quicksand(collision.gameObject));
                }
                break;
            case 26:
                if (!_isShielded)
                {
                    GroundBoost(collision.gameObject);
                }
                break;
            
        }
        if (collision.gameObject.CompareTag("Flag")) return; //Exception for Flag
        if (collision.gameObject.CompareTag("Traps")) return; //Exception for Traps
        if (collision.gameObject.layer == 30) return;
        GameManager.Instance.AudioManager.PlayEffect("power-up");
        Destroy(collision.gameObject);
    }
    private void NormalizeMaxSpeedVar()
    {
        _maxSpeed = _normalMaxSpeed;
    }
    public IEnumerator ShieldRemover()
    {
        yield return new WaitForSeconds(_shieldTimer);
        _isShielded = false;
    }

    private void GroundBoost(GameObject collObj)
    {
        
        Rb.velocity = Vector3.zero;
        Vector3 targetDirection = collObj.transform.forward;
        StartCoroutine(RotateTowardsDirection(targetDirection, rotateTowardsSpeed));
        Vector3 push = collObj.transform.forward * _boostForce;
        Rb.AddForce(push, ForceMode.Impulse);
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
        float startAltitude = _camerasGO.transform.position.y;

        while (_camerasGO.transform.position.y > _blockHeight)
        {
            if (IsOutQuicksand(collObj))
            {
                _camerasGO.transform.position = new Vector3(_camerasGO.transform.position.x, _camerasOriginPosition, _camerasGO.transform.position.z);
                
                yield break;                        //float closestDistance = Vector3.Distance(transform.position, actualClosestFlag.transform.position);
            }
            float progress = (Time.time - startTime) / _blockDuration;
            float interpolation = Mathf.Pow(progress, 2);
            float newY = Mathf.Lerp(startAltitude, _blockHeight, interpolation * _descentSpeed);
            _camerasGO.transform.position = new Vector3(_camerasGO.transform.position.x, newY, _camerasGO.transform.position.z);

            yield return null;
        }
        
        Rb.velocity = Vector3.zero;
        _isAbleToMove = false;
        
        yield return new WaitForSeconds(_blockDuration);
        
        _isAbleToMove = true;
        _camerasGO.transform.position = new Vector3(_camerasGO.transform.position.x, _camerasOriginPosition, _camerasGO.transform.position.z);
    }
    private bool IsOutQuicksand(GameObject collObj)
    {
        if (Vector3.Distance(transform.position, collObj.transform.position) > quickSandMaxDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }



}
