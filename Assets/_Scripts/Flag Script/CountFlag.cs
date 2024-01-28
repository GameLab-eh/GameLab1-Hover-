using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountFlag : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] int flagCount;

    public void Increment()
    {
        flagCount++;
    }
    public void Decrement(bool isEnemy)
    {
        if (flagCount > 0)
        {
            flagCount--;
            GameManager.Instance.DecrementFlagCount(isEnemy);
        }
    }
}
