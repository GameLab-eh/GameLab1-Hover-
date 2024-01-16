using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flag : MonoBehaviour
{
    //for Designer
    [Header("Flag Spawn Point")]
    [SerializeField] List<GameObject> _ListObj;

    [Header("Variables")]
    [SerializeField, Range(0, 10)] float _delayStartSpawn = 0f;
    [SerializeField, Range(0, 10)] float _delaySpawn = 0f;
    [SerializeField] Collider _collider;

    //general variables
    int _scoreOnHit = 1;

    public delegate void Hit(int score);
    public static event Hit FlagHit = null;

    private void Awake()
    {
        _collider = GetComponent<Collider>();

        //check Designer error
        if (!_ListObj.Any()) Debug.LogError("The spawn points list for the flag object is empty.");
    }

    private void Start()
    {
        Invoke(nameof(spawn), _delayStartSpawn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 31) /*31 = player*/
        {
            Invoke(nameof(spawn), _delaySpawn);
            FlagHit?.Invoke(_scoreOnHit);
            gameObject.SetActive(false);
        }
    }

    private void spawn()
    {
        Vector3 _olfPosition = this.transform.position;
        Vector3 _newPosition;

        int _tmp = Random.Range(0, _ListObj.Count);
        if (_ListObj.Count > 1)
        {
            do
            {
                _newPosition = _ListObj[_tmp].transform.position;
            } while (_newPosition == _olfPosition);
        }
        else
        {
            _newPosition = _ListObj[0].transform.position;
        }

        this.transform.position = _newPosition;
        gameObject.SetActive(true);
    }
}
