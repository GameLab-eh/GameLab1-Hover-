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
    [SerializeField] GameObject _flagEnemyModel;

    private int _numberFlags;

    private void Awake()
    {
        //check Designer error
        if (!_ListObj.Any()) Debug.LogError("The spawn points list for the Enemy Flag Generator object is empty.");
    }

    private void Start()
    {
        _numberFlags = GameManager.Instance.GetNumberFlagsEnemy();
        _numberFlags = Mathf.Clamp(_numberFlags, 0, _ListObj.Count);
        GameManager.Instance.SetNumberFlagsEnemy(_numberFlags);
        List<GameObject> _tmpList = _ListObj;
        for (int i = 0; i < _numberFlags; i++)
        {
            int _tmp = Random.Range(0, _tmpList.Count);
            Instantiate(_flagEnemyModel, _tmpList[_tmp].transform.position, Quaternion.identity, transform);
            _tmpList.Remove(_tmpList[_tmp]);
        }
    }
}
