using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class AiPooler : MonoBehaviour
{
    [Header("Spawn Settings")] [SerializeField, Tooltip("insert enemys")]
    private GameObject[] _enemyPrefab;

    [SerializeField] private List<Transform> _spawnPointList;

    [SerializeField, Tooltip("Number of spawns for each enemy")]
    private int _spawnCounter;

    private void Start()
    {
        if ((_enemyPrefab.Length * _spawnCounter) > _spawnPointList.Count)
        {
            return;
        }

        foreach (GameObject enemy in _enemyPrefab)
        {
            for (int i = 0; i < _spawnCounter; i++)
            {
                int indexNumber = Random.Range(0, _spawnPointList.Count);
                Instantiate(enemy, _spawnPointList[index: indexNumber]);
                _spawnPointList.RemoveAt(indexNumber);
            }
        }
    }
}
