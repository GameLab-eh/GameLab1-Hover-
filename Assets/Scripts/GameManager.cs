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

    public void IncrementScore(int value)
    {
        flagPlayer += value;
    }
    public void IncrementScoreE(int value)
    {
        flagEnemy += value;
    }
    public void OnEnable()
    {
        Flag.FlagHit += IncrementScore;
        FlagE.FlagHit += IncrementScoreE;
    }

    #endregion

}
