using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class AiPooler : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField, Tooltip("insert enemys")]
    private GameObject[] _enemyPrefab;

    [SerializeField] private List<Transform> _spawnPointList;

    private int[] _spawnCounter = new int[2];

    private void Start()
    {
        _spawnCounter[0] = GameManager.Instance.GetNumberChaserBot();
        _spawnCounter[1] = GameManager.Instance.GetNumberScoutBot();

        int _spawnCounterTotal = _spawnCounter[0] + _spawnCounter[1];

        if (_spawnCounterTotal > _spawnPointList.Count)
        {
            return;
        }

        int tmp = 0;
        foreach (GameObject enemy in _enemyPrefab)
        {
            for (int i = 0; i < _spawnCounter[tmp]; i++)
            {
                int indexNumber = Random.Range(0, _spawnPointList.Count);
                Instantiate(enemy, _spawnPointList[index: indexNumber]);
                _spawnPointList.RemoveAt(indexNumber);
            }

            tmp++;
        }
    }
}
