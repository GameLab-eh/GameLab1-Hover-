using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapping : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] Camera _mainCamera;
    [SerializeField] Camera _backCamera;
    [SerializeField, Range(-5, 5)] float _heightPoisiton;
    [SerializeField, Tooltip("It's the number of raycasts to generate")] int _density = 5;
    [SerializeField, Tooltip("It's the max distance to render the raycasts")] float _maxRayRange;

    [Header("Debug")]
    [SerializeField, Tooltip("It's the list of active objects in minimap")] private List<GameObject> _miniMapObjActivated = new List<GameObject>();
    [SerializeField, Tooltip("Debug viwe of raycast")] bool _view;

    private void Update()
    {
        RaycastMapping(_mainCamera);
        RaycastMapping(_backCamera);
    }

    void RaycastMapping(Camera _camera)
    {
        float _fieldOfView = _camera.fieldOfView;
        float _angularInterval = _fieldOfView / (float)(_density - 1);

        for (int i = 0; i < _density; i++)
        {
            float _rayAngle = -_fieldOfView / 2f + i * _angularInterval;
            Vector3 _rayDirection = Quaternion.Euler(0f, _rayAngle, 0f) * _camera.transform.forward;

            Vector3 _pivot = new(_camera.transform.position.x, _heightPoisiton, _camera.transform.position.z);

            Ray ray = new(_pivot, _rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _maxRayRange))
            {
                if (hitInfo.collider != null)
                {
                    Transform childTransform = hitInfo.transform.GetChild(0);
                    if (childTransform != null && !childTransform.gameObject.activeSelf)
                    {
                        childTransform.gameObject.SetActive(true);
                        _miniMapObjActivated.Add(childTransform.gameObject);
                    }
                }
            }
            if (_view) Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red, 0.1f);
            else if (!_view && hitInfo.collider != null) Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.red, 0.1f);
            else Debug.DrawRay(ray.origin, ray.direction * _maxRayRange, Color.red, 0.1f);
        }
    }

    #region Event

    public void OnEnable()
    {
        PlayerController.EraseMap += EraseMap;
    }
    public void OnDisable()
    {
        PlayerController.EraseMap -= EraseMap;
    }

    #endregion

    private void EraseMap()
    {
        foreach (GameObject obj in _miniMapObjActivated)
        {
            obj.gameObject.SetActive(false);
        }
        _miniMapObjActivated.Clear();
    }

}
