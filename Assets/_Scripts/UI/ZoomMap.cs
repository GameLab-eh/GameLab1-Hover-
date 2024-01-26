using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomMap : MonoBehaviour
{
    private Camera mainCamera;
    private float _factor = 8f;
    private string cameraTag = "MiniMapCamera"; // Set the tag of your camera
    private float _minZoom;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag(cameraTag)?.GetComponent<Camera>();
        _minZoom = mainCamera.GetComponent<MiniMapZoom>().GetMinZoom();
    }

    void Update()
    {
        float zoom = mainCamera.transform.position.y;

        float scaleFactor = 1f + (zoom - _minZoom) / (_minZoom * _factor);
        Vector3 localScale = this.GetComponent<OriginLocalScale>().GetLocalScale();

        float newX = localScale.x * scaleFactor;

        this.transform.localScale = new Vector3(newX, newX, newX);
    }
}
