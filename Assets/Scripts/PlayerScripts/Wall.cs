using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Wall : MonoBehaviour
{
    private float secondBeforeDestroy;
    private Vector3 _startingPosition;
    private bool _isSpawned;

    private void Start()
    {
        secondBeforeDestroy = GameManager.Instance.GetWallDelayDestroy();
    }
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
