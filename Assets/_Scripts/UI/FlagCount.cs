using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCount : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] TMPro.TMP_Text _flagPlayer;
    [SerializeField] TMPro.TMP_Text _flagEnemy;

    [SerializeField] string _flagEmpty = "♦";
    [SerializeField] string _flagFill = "♥";

    int _flagToCapture;
    int _flagsEnemy;

    private void Awake()
    {
        _flagPlayer.text = string.Empty;
        _flagEnemy.text = string.Empty;
    }

    private void Start()
    {
        _flagToCapture = GameManager.Instance.GetNumberFlagsToCapture();
        _flagsEnemy = GameManager.Instance.GetNumberFlags();
        for (int i = 0; i < _flagToCapture; i++) _flagPlayer.text += _flagEmpty;
        for (int i = 0; i < _flagsEnemy; i++) _flagEnemy.text += _flagEmpty;
    }

    private void Update()
    {
        _flagPlayer.text = string.Empty;
        _flagEnemy.text = string.Empty;
        for (int i = 0; i < GameManager.Instance.GetFlagsToCapture(); i++) _flagPlayer.text += _flagFill;
        for (int i = 0; i < _flagToCapture - GameManager.Instance.GetFlagsToCapture(); i++) _flagPlayer.text += _flagEmpty;
        for (int i = 0; i < GameManager.Instance.GetFlagsEnemy(); i++) _flagEnemy.text += _flagFill;
        for (int i = 0; i < _flagToCapture - GameManager.Instance.GetFlagsEnemy(); i++) _flagEnemy.text += _flagEmpty;
    }
}
