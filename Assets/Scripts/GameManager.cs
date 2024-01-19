using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Scripting;

[RequireComponent(typeof(GameManager))]
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
    [SerializeField] bool inputSystem;

    //for Designer
    [Header("Power-Up  Settings")]
    [SerializeField, Min(0), Tooltip("is the duration of wall")] float _wallDelayDestroy;
    [SerializeField, Min(0), Tooltip("is the duration of invisibility")] float _invisibilityDuration;
    [SerializeField, Min(0), Tooltip("is the duration of shield")] float _shieldDuration;
    [SerializeField, Min(0), Tooltip("is the duration of stoplight")] float _stoplightDuration;

    [Header("Value for score")]
    [SerializeField, Min(0)] int _flagValue;
    [SerializeField, Min(0)] int _flagEnemyValue;
    [SerializeField] List<Difficulty> _difficultyScoreValue;

    [Header("Level Settings")]
    [SerializeField] int _currentLevel;
    [SerializeField] List<Level> Levels;

    //Local variables
    [Header("Local Variables")]
    [SerializeField] static bool gameIsPaused;
    [SerializeField] static bool gameIsEnded;


    void Awake()
    {
        if (TimerManagerInstance == null)
        {
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

        if (flagPlayer == Levels[_currentLevel].flagsToCapture)
        {
            //win
            Debug.Log("win");
            gameIsEnded = true;
            EndGame();
        }
    }
    public void IncrementFlagCountEnemy(int value)
    {
        flagEnemy += value;
        score += value * _flagEnemyValue;

        if (flagEnemy == Levels[_currentLevel].flagsEnemy)
        {
            //lose
            gameIsEnded = true;
            EndGame();
        }
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
        Flag.FlagHit += IncrementFlagCount;
        FlagE.FlagHit += IncrementFlagCountEnemy;
        PlayerController.PlayerSpeed += PlayerSpeed;
    }
    public void OnDisable()
    {
        Flag.FlagHit -= IncrementFlagCount;
        FlagE.FlagHit -= IncrementFlagCountEnemy;
        PlayerController.PlayerSpeed -= PlayerSpeed;
    }

    #endregion

    #region Set

    public void SetInputSystem(bool value) => inputSystem = value;

    public void SetNumberFlagsEnemy(int value) => Levels[_currentLevel].flagsEnemy = value;

    #endregion

    #region Get

    public float GetShieldDuration() => _invisibilityDuration;

    public float GetStoplightDuration() => _invisibilityDuration;

    public float GetInvisibilityDuration() => _invisibilityDuration;

    public float GetWallDelayDestroy() => _wallDelayDestroy;

    public bool GetInputSystem() => inputSystem;

    public int GetScore() => score;

    public float GetPlayerSpeed() => playerSpeed;

    public int GetFlagsToCapture() => flagPlayer;

    public int GetFlagsEnemy() => flagEnemy;

    public int GetNumberFlagsToCapture() => Levels[_currentLevel].flagsToCapture;

    public int GetNumberFlagsEnemy() => Levels[_currentLevel].flagsEnemy;

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
    public int flagsToCapture;
    public int flagsEnemy;
    public int levelBonus;

    //Bot
    public int botGreen;
    public int botBlue;

    public Level(int flagsToCapture, int flagsEnemy, int levelBonus, int botGreen, int botBlue)
    {
        this.flagsToCapture = flagsToCapture;
        this.flagsEnemy = flagsEnemy;
        this.levelBonus = levelBonus;
        this.botGreen = botGreen;
        this.botBlue = botBlue;
    }
}