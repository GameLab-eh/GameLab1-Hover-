using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }

    //variabili statiche
    public static TimerManager TimerManagerInstance { get; private set; }

    void Awake()
    {
        //ricordarsi di assegnare il valore delle variabili statiche

        if (TimerManagerInstance == null)
        {
            // Aggiungi il componente TimerManager dinamicamente al GameObject associato al GameManager
            TimerManagerInstance = gameObject.AddComponent<TimerManager>();
        }

        #region Singleton

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            Destroy(transform.root.gameObject);
            return;
        }

        #endregion
    }
}
