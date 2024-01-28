using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountFlag : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] List<GameObject> _flag;

    public void Increment(GameObject flag)
    {
        _flag.Add(flag);
    }
    public void Decrement(bool isEnemy)
    {
        if (_flag.Count > 0)
        {
            _flag.RemoveAt(_flag.Count - 1);
            GameManager.Instance.DecrementFlagCount(isEnemy);
        }
    }

    public GameObject GetModel()
    {
        if (_flag.Count > 0) return _flag[_flag.Count - 1];
        return null;
    }
}
