using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagMechanic : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] LayerMask _layerMask;
    [SerializeField] bool _isEnemy;

    [Header("Animation")]
    [SerializeField, Tooltip("time of hidden and visibility on minimap")] float _delay;

    public delegate void Hit(bool isEnemy);
    public static event Hit FlagHit = null;

    private GameObject firstChild;

    private void Start()
    {
        firstChild = transform.GetChild(0).gameObject;
        StartCoroutine(ActivateDeactivateIntermittently());
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((_layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if (_isEnemy)
            {
                Renderer rend = other.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Renderer>();
                rend.material.color = Color.yellow;
            }
            FlagHit?.Invoke(_isEnemy);
            gameObject.SetActive(false);
        }
    }

    System.Collections.IEnumerator ActivateDeactivateIntermittently()
    {
        while (true)
        {
            firstChild.SetActive(true);

            yield return new WaitForSeconds(_delay);

            // Deactivate the first child
            firstChild.SetActive(false);

            yield return new WaitForSeconds(_delay);
        }
    }
}
