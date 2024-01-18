using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCount : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] TMPro.TMP_Text _flagRed;
    [SerializeField] TMPro.TMP_Text _flagBlue;

    [SerializeField] string _flagRedNull = "♦";
    [SerializeField] string _flagRedFill = "♥";
    [SerializeField] string _flagBlueNull = "♠";
    [SerializeField] string _flagBlueFill = "♣";

    int _flagToCapture;
    int _flagsEnemy;

    private void Awake()
    {
        _flagRed.text = string.Empty;
        _flagBlue.text = string.Empty;
    }

    private void Start()
    {
        _flagToCapture = GameManager.Instance.GetNumberFlagsToCapture();
        _flagsEnemy = GameManager.Instance.GetNumberFlagsEnemy();
        for (int i = 0; i < _flagToCapture; i++) _flagRed.text += _flagRedNull;
        for (int i = 0; i < _flagsEnemy; i++) _flagBlue.text += _flagBlueNull;
    }

    private void Update()
    {
        _flagRed.text = string.Empty;
        _flagBlue.text = string.Empty;
        for (int i = 0; i < GameManager.Instance.GetFlagsToCapture(); i++) _flagRed.text += _flagRedFill;
        for (int i = 0; i < _flagToCapture - GameManager.Instance.GetFlagsToCapture(); i++) _flagRed.text += _flagRedNull;
        for (int i = 0; i < GameManager.Instance.GetFlagsEnemy(); i++) _flagBlue.text += _flagBlueFill;
        for (int i = 0; i < _flagToCapture - GameManager.Instance.GetFlagsEnemy(); i++) _flagBlue.text += _flagBlueNull;
    }
}
