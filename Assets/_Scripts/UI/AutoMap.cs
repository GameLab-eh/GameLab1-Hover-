using System.Collections.Generic;
using UnityEngine;

public class AutoMap : MonoBehaviour
{
    [Header("variables")]
    [SerializeField] List<GameObject> _gameObjects;
    [SerializeField] LayerMask _miniMapLayer;
    [SerializeField] Material _material;

    [Header("Debug")]
    [SerializeField] bool _debugview;

    void Start()
    {
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            CalcolaIpotenusaEAngolo(_gameObjects[i]);
        }
        //remove allocate memory
        _gameObjects.Clear();
    }

    void CalcolaIpotenusaEAngolo(GameObject originalObject)
    {
        float length = originalObject.GetComponent<MeshRenderer>().bounds.size.z;
        float width = originalObject.GetComponent<MeshRenderer>().bounds.size.x;

        float _hypotenuse = Mathf.Sqrt(Mathf.Pow(length, 2) + Mathf.Pow(width, 2));
        float angle = Mathf.Rad2Deg * Mathf.Atan2(width, length);

        GameObject _newMesh = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _newMesh.name = "Mesh";

        _newMesh.transform.localScale = new Vector3(0.5f, _hypotenuse, 0.5f);

        if(originalObject.GetComponent<NegativeAngle>() != null) angle = -angle;
        if (originalObject.GetComponent<Exception>() != null)
        {
            angle += originalObject.GetComponent<Exception>().isNegative ? +12f : -12f;
        }

        _newMesh.transform.SetPositionAndRotation(originalObject.transform.position, Quaternion.Euler(90f, angle, 0f));

        _newMesh.layer = (29 << _miniMapLayer);

        _newMesh.transform.parent = originalObject.transform;

        _newMesh.GetComponent<MeshRenderer>().material = _material;

        _newMesh.SetActive(false);
    }
}
