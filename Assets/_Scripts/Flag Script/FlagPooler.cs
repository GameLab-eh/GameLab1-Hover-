using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlagPooler : MonoBehaviour
{
    [Header("Spawn Point")]
    [SerializeField] List<GameObject> _playerPoints;
    [SerializeField] List<GameObject> _enemyPoints;

    [Header("3D Models")]
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _enemy;

    private int _numberFlags;

    private List<GameObject> _clonePlayerPoints;
    private List<GameObject> _cloneEnemyPoints;

    private void Awake()
    {
        //check Designer error
        if (!_playerPoints.Any()) Debug.LogError("The spawn points list Flag Generator object is empty");
        _clonePlayerPoints = _playerPoints;
        if (!_enemyPoints.Any()) Debug.LogError("The spawn points list Flag Generator object is empty");
        _cloneEnemyPoints = _enemyPoints;
    }

    private void Start()
    {
        _numberFlags = GameManager.Instance.GetNumberFlags();

        Spawn(_playerPoints, _player, _numberFlags, false);
        Spawn(_enemyPoints, _enemy, _numberFlags, false);
    }

    private void Spawn(List<GameObject> _ListObj, GameObject _model, int _toSpawn, bool _isOld)
    {
        for (int i = 0; i < _toSpawn; i++)
        {
            int _tmp = Random.Range(0, _ListObj.Count);
            if (_isOld)
            {
                _model.transform.position = _ListObj[_tmp].transform.position;
                _model.SetActive(true);
            }
            else Instantiate(_model, _ListObj[_tmp].transform.position, Quaternion.identity, transform);
            _ListObj.Remove(_ListObj[_tmp]);
        }
    }

    void OnEnable()
    {
        FlagRemover.Remove += ReSpawn;
    }

    private void OnDisable()
    {
        FlagRemover.Remove -= ReSpawn;
    }

    void ReSpawn(CountFlag container, bool isEnemy)
    {
        if (container.GetModel() == null) return;

        if (!_playerPoints.Any()) _playerPoints = _clonePlayerPoints;
        if (!_enemyPoints.Any()) _enemyPoints = _cloneEnemyPoints;

        Spawn(isEnemy ? _enemyPoints : _playerPoints, container.GetModel(), 1, true);

        container.Decrement(isEnemy);
    }
}
