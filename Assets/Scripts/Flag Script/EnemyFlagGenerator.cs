using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFlagGenerator : MonoBehaviour
{
    //for Designer
    [Header("Flag Spawn Point")]
    [SerializeField] List<GameObject> _ListObj;

    [Header("Variables")]
    [SerializeField, Range(0, 50)] int _flagEnum;
    [SerializeField] GameObject _flagE;

    private void Awake()
    {
        //check Designer error
        if (!_ListObj.Any()) Debug.LogError("The spawn points list for the Enemy Flag Generator object is empty.");
    }

    private void Start()
    {
        _flagEnum = Mathf.Clamp(_flagEnum, 0, _ListObj.Count);
        List<GameObject> _tmpList = _ListObj;
        for (int i = 0; i < _flagEnum; i++)
        {
            int _tmp = Random.Range(0, _tmpList.Count);
            Instantiate(_flagE, _tmpList[_tmp].transform.position, Quaternion.identity, transform);
            _tmpList.Remove(_tmpList[_tmp]);
        }
    }
}
