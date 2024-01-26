using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginLocalScale : MonoBehaviour
{
    private Vector3 _myLocalScale = Vector3.one;

    public Vector3 GetLocalScale() { return _myLocalScale; }

    public void SetLocalScale(Vector3 value) => _myLocalScale = value;
}
