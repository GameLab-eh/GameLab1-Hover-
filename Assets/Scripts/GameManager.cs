using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Static variables
    public static GameManager Instance { get; private set; }
    public static TimerManager TimerManagerInstance { get; private set; }

    //Global variables
    [Header("Game Variables")]
    [SerializeField, Tooltip("It's count of Capture Flag")] private int flagPlayer;
    [SerializeField, Tooltip("It's count of Capture Flag from Enemy")] private int flagEnemy;
    [SerializeField, Tooltip("It's the game score")] private int score;
    [SerializeField, Tooltip("It's the speed of the player")] private float playerSpeed;
    [SerializeField, Range(0, 2), Tooltip("Level of Difficulty")] private int difficulty = 1;

    //Local variables
    [Header("Local Variables")]
    [SerializeField] static bool gameIsPaused;
    [SerializeField] static bool gameIsEnded;

    //for Designer
    [Header("Value for score")]
    [SerializeField, Min(0)] int _flagValue;
    [SerializeField, Min(0)] int _flagEnemyValue;
    [SerializeField] List<Difficulty> _difficultyScoreValue;

    //Levels variables
    [Header("Level Settings")]
    [SerializeField] int _currentLevel;
    [SerializeField] List<Level> Levels;


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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    #region Event

    public void IncrementFlagCount(int value)
    {
        flagPlayer += value;
        score += value * _flagValue;

        if (value == Levels[_currentLevel].flagsToCapture)
        {
            gameIsEnded = true;
            EndGame();
        }
    }
    public void IncrementFlagCountEnemy(int value)
    {
        flagEnemy += value;
        score += value * _flagEnemyValue;
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

    #region game mecchanic



    #endregion

    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    void EndGame()
    {
        if (gameIsEnded)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

}

[Serializable]
public class Difficulty
{
    public GameObject gameObject;
    public int easy;
    public int medium;
    public int hard;

    public Difficulty(GameObject obj, int v1, int v2, int v3)
    {
        gameObject = obj;
        easy = v1;
        medium = v2;
        hard = v3;
    }
}

[Serializable]
public class Level
{
    public GameObject gameObject;
    public int flagsToCapture;
    public int levelBonus;

    //Bot
    public int botGreen;
    public int botBlue;

    public Level(GameObject gameObject, int flagsToCapture, int levelBonus, int botGreen, int botBlue)
    {
        this.gameObject = gameObject;
        this.flagsToCapture = flagsToCapture;
        this.levelBonus = levelBonus;
        this.botGreen = botGreen;
        this.botBlue = botBlue;
    }
}