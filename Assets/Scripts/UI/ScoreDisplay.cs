using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] int _score;
    [SerializeField] TMP_Text _scoreDisplay;

    void Update()
    {
        _score = GameManager.Instance.GetScore();
        EditScore();
    }

    void EditScore()
    {
        _scoreDisplay.text = "" + _score.ToString("D6");
    }
}
