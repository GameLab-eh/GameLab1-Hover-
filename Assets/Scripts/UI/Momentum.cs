using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Momentum : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] GameObject _momentum;
    [SerializeField] GameObject _player;
    [SerializeField] Camera _mainCamera;
    public float dampingFactor = 5f;

    void Update()
    {
        Rigidbody rb = _player.GetComponent<Rigidbody>();
        Vector3 velocity = rb.velocity;
        Vector3 dir = velocity.normalized;
        Vector3 _visualDirection = _mainCamera.transform.forward;
        float angle = Vector3.SignedAngle(dir, _visualDirection, Vector3.up);
        _momentum.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
