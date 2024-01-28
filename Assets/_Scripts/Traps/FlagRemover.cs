using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagRemover : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CountFlag>() != null)
        {
            other.gameObject.GetComponent<CountFlag>().Decrement(other.gameObject.layer == 30);
        }
    }
}
