using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMechanic : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] LayerMask _layerMask;
    [SerializeField] bool _isEnemy;

    public delegate void Hit(bool isEnemy);
    public static event Hit FlagHit = null;

    private void OnTriggerEnter(Collider other)
    {
        if ((_layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            FlagHit?.Invoke(_isEnemy);
            gameObject.SetActive(false);
        }
    }
}
