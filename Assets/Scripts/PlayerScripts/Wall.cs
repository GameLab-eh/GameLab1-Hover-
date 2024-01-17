using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] float secondBeforeDestroy = 5f;
    private Vector3 _startingPosition;
    private bool _isSpawned;

    private void Update()
    {
        if (!_isSpawned)
        {
            if (gameObject.activeInHierarchy)
            {
                _isSpawned = true;
                Invoke("DeactivateWall", secondBeforeDestroy);
            }
        }
    }

    private void DeactivateWall()
    {
        gameObject.SetActive(false);
        _isSpawned = false;
    }

}
