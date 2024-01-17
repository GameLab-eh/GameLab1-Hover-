using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //variabili statiche
    public static TimerManager TimerManagerInstance { get; private set; }

    //variabili globali
    [SerializeField] private int flagPlayer;
    [SerializeField] private int flagEnemy;
    [SerializeField] private int score;
    [SerializeField] private float playerSpeed;

    void Awake()
    {
        //ricordarsi di assegnare il valore delle variabili statiche

        if (TimerManagerInstance == null)
        {
            // Aggiungi il componente TimerManager dinamicamente al GameObject associato al GameManager
            TimerManagerInstance = gameObject.AddComponent<TimerManager>();
        }

        #region Singleton

        if (Instance != null)
        {
            Destroy(transform.root.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(transform.root.gameObject);

        #endregion
    }

    #region Event

    public void IncrementFlagCount(int value)
    {
        flagPlayer += value;
    }
    public void IncrementFlagCountEnemy(int value)
    {
        flagEnemy += value;
    }
    public void IncrementScore(int value)
    {
        score += value;
    }
    public void PlayerSpeed(float value)
    {
        playerSpeed = value;
    }
    public void OnEnable()
    {
        Flag.FlagHit += IncrementScore;
        FlagE.FlagHit += IncrementFlagCountEnemy;
        PlayerController.PlayerSpeed += PlayerSpeed;
    }
    public void OnDisable()
    {
        Flag.FlagHit -= IncrementScore;
        FlagE.FlagHit -= IncrementFlagCountEnemy;
        PlayerController.PlayerSpeed -= PlayerSpeed;
    }

    #endregion

    #region return

    public int GetScore()
    {
        return score;
    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    #endregion

}
