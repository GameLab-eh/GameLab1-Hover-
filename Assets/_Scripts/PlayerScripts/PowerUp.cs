using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    //Power Up Spawner

    [Header("Spawn Settings")]
    [SerializeField, Tooltip("Insert Every Power up")] private GameObject[] _powerUpPrefab;

    [SerializeField] private List<Transform> _spawnPointList;

    private int _spawnCounter;

    private void Awake()
    {
        _spawnCounter = GameManager.Instance.GetNumberPowerUp();

        if ((_powerUpPrefab.Length * _spawnCounter) > _spawnPointList.Count)
        {
            Debug.LogError("The number of spawn points in the power-up spawn point list is insufficient");
            return;
        }

        foreach (GameObject powerUps in _powerUpPrefab)
        {
            for (int i = 0; i < _spawnCounter; i++)
            {
                int indexNumber = Random.Range(0, _spawnPointList.Count);
                Instantiate(powerUps, _spawnPointList[index: indexNumber]);
                _spawnPointList.RemoveAt(indexNumber);
            }
        }
    }
}