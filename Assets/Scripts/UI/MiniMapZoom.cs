using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapZoom : MonoBehaviour
{
    //for deisgner
    [Header("Variables")]
    [SerializeField, Range(0, 100)] float _maxZoom = 10f;
    [SerializeField, Range(0, 99)] float _minZoom = 0f;
    [SerializeField] float _defaultZoom;
    [SerializeField] float _speedZoom;

    [Header("Debug")]
    [SerializeField] float _zoom;

    //basic movement variables
    private float _miniMapZoom;

    private void Awake()
    {
        if (_maxZoom < _minZoom) _maxZoom = _minZoom + 1f;
        _defaultZoom = Mathf.Clamp(_defaultZoom, _minZoom, _maxZoom);

        //Debug
        _zoom = transform.position.y;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.parent.position.x, _defaultZoom, transform.parent.position.z);
    }


    void Update()
    {
        _miniMapZoom = Input.GetAxisRaw("Zoom");
        Zoom();
    }

    private void Zoom()
    {
        Vector3 _newPosition = transform.position + Vector3.down * _miniMapZoom;
        //if (_newPosition.y > _maxZoom) return;
        //else if (_newPosition.y < _minZoom) return;
        _newPosition.y = Mathf.Clamp(_newPosition.y, _minZoom, _maxZoom);
        transform.position = Vector3.Lerp(transform.position, _newPosition, _speedZoom * Time.deltaTime);

        //Debug
        _zoom = transform.position.y;
    }
}
