using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlagE : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] Collider _collider;

    //general variables
    int _scoreOnHit = 1;

    public delegate void Hit(int score);
    public static event Hit FlagHit = null;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 30) /*30 = enemy*/
        {
            FlagHit?.Invoke(_scoreOnHit);
            gameObject.SetActive(false);
        }
    }
}
