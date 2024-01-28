using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagRemover : MonoBehaviour
{
    public delegate void RemoveFlag(bool isEnemy);
    public static event RemoveFlag Remove = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CountFlag>() != null)
        {
            bool _isEnemy = other.gameObject.layer == 30;
            other.gameObject.GetComponent<CountFlag>().Decrement(_isEnemy);

            Remove?.Invoke(_isEnemy);
        }
    }
}
