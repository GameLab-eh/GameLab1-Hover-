using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
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
    private bool _isGrounded = true;
    private bool _isInvisible = false;

    //general variables
    private int _playerScore;

    //basic movement variables
    private float _horizontal;
    private float _vertical;
    [Header("Physics")]
    [SerializeField, Min (0)] float movementSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
    private float _normalMaxSpeed; //used for store MaxSpeedVariable to restore to default when changed (currently not used)

    //for buff and malus variables
    //jump
    [SerializeField] float jumpPower;
    [SerializeField] float _jumpStack = 10;
    [SerializeField] float _invisibilityStack = 10;
    


    [field:SerializeField]public Rigidbody Rb { get; private set; }

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
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _isGrounded = true;
        }
    }

    private void rotate()
    {
        float angle = _horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, angle);
    }

    private void Move()
    {
        if (Rb.velocity.magnitude < maxSpeed)
        {
            Vector3 move = transform.forward * _vertical * movementSpeed;
            Rb.AddForce(move, ForceMode.Force);
        }
    }
    
    private void Jump()
    {
        if(_isGrounded && _jumpStack > 0)
        {
            Rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            _jumpStack--;
            _isGrounded = false;
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
}
