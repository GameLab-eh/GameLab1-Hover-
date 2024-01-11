using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //general variables
    [SerializeField] private Rigidbody rb;
    private bool isPlayerAbleToMove = true;
    private int _playerScore;

    //basic movement variables
    private float _horizontal;
    private float _vertical;
    [SerializeField] float movementSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
    private float _normalMaxSpeed; //used for store MaxSpeedVariable to restore to default when changed (currently not used)

    public Rigidbody Rb { get => rb; private set => rb = value; }

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isPlayerAbleToMove)
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            rotate();
        }
    }


    void FixedUpdate()
    {
        Move();
    }

    private void rotate()
    {
        float angle = _horizontal * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, angle);
    }

    private void Move()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector3 move = transform.forward * _vertical * movementSpeed;
            Rb.AddForce(move, ForceMode.Force);
        }
    }
}
