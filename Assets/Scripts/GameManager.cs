using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager Instance { get; private set; }

    //variabili statiche



    void Awake()
    {
        //ricordarsi di assegnare il valore delle variabili statiche


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
