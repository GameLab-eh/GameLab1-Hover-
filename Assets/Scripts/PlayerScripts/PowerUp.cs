using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    //Power Up Spawner

    [Header("Spawn Settings")] 
    [SerializeField,Tooltip("Insert Every Power up")] private GameObject[] _powerUpPrefab;

    [SerializeField] private List<Transform> _spawnPointList;

    [SerializeField, Tooltip("number of spawns for each power up")]
    private int _spawnCounter;

    private void Awake()
    {
        foreach (GameObject powerUps in _powerUpPrefab)
        {
            for (int i = 0; i < _spawnCounter; i++)
            {
                int indexNumber = Random.Range(0, _spawnPointList.Count);
                Instantiate(powerUps, _spawnPointList[indexNumber]);
                _spawnPointList.RemoveAt(indexNumber);
            }
        }
    }
}   