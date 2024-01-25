using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderMap : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = new Vector3(114.7f, 0f, -75.5f);

        Debug.Log("achievement unlocked\nundermap");
    }
}
