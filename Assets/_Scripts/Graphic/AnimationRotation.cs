using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRotation : MonoBehaviour
{

    private float rotationSpeed = 30f;
    private float oscillationSpeed = 1f;
    private float oscillationHeight = 0.2f; 

    private float initialY;

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float oscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationHeight;
        transform.position = new Vector3(transform.position.x, initialY + oscillation, transform.position.z);
    }
}
