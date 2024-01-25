using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(GameManager))]
public class GameManager : MonoBehaviour
{
    //Static variables
    public static GameManager Instance { get; private set; }

    [Header("System Variables")]
    [SerializeField, Range(0, 2), Tooltip("Level of Difficulty")] private int difficulty = 1;
    [SerializeField] bool inputSystem;

    [Header("UI Variables")]
    [SerializeField, Tooltip("It's count of Capture Flag")] private int flagPlayer;
    [SerializeField, Tooltip("It's count of Capture Flag from Enemy")] private int flagEnemy;
    [SerializeField, Tooltip("It's the game score")] private int score;
    [SerializeField, Tooltip("It's the speed of the player")] private float playerSpeed;

    //for Designer
    [Header("Power-Up  Settings")]
    [SerializeField, Min(0), Tooltip("is the duration of wall")] float _wallDelayDestroy;
    [SerializeField, Min(0), Tooltip("is the duration of invisibility")] float _invisibilityDuration;
    [SerializeField, Min(0), Tooltip("is the duration of shield")] float _shieldDuration;
    [SerializeField, Min(0), Tooltip("is the duration of stoplight")] float _stoplightDuration;

    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _gameWin;
    [SerializeField] GameObject _gameOver;

    [Header("Level Settings")]
    [SerializeField] int _currentLevel;
    [SerializeField] List<Level> Levels;

    [Header("Debug")]
    [SerializeField] static bool gameIsPaused;
    [SerializeField] static bool gameIsEnded;


    void Awake()
    {

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
        if (Input.GetKeyDown(KeyCode.Escape) && _currentLevel != 0)
        {
            PauseGame();
        }
    }

    #region Event

    public void OnEnable()
    {
        FlagMechanic.FlagHit += IncrementFlagCount;
        PlayerController.PlayerSpeed += PlayerSpeed;
        OptionManager.difficulty += SetDifficulty;
        OptionManager.input += SetInputSystem;
    }
    public void OnDisable()
    {
        FlagMechanic.FlagHit -= IncrementFlagCount;
        PlayerController.PlayerSpeed -= PlayerSpeed;
        OptionManager.difficulty -= SetDifficulty;
        OptionManager.input -= SetInputSystem;
    }

    #endregion

    #region Set
    public void SetGameMenu(GameObject gameMenu) => _menu = gameMenu;

    public void SetInputSystem(bool value) => inputSystem = value;

    public void SetCurrentLevel(int value) => _currentLevel = value;
    public void IncrementCurrentLeve() => _currentLevel++;

    public void SetNumberFlagsEnemy(int value) => Levels[_currentLevel].flags = value;

    public void SetDifficulty(int value) => difficulty = value;

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

    public int GetNumberFlagsToCapture() => Levels[_currentLevel].flags;

    public int GetNumberFlagsEnemy() => Levels[_currentLevel].flags;
    public int GetNumberPowerUp() => Levels[_currentLevel].powerUp;
    public int GetNumberChaserBot() => Levels[_currentLevel].chaserBot;
    public int GetNumberScoutBot() => Levels[_currentLevel].scoutBot;
    public int GetLevel() => Levels.Count;

    #endregion

    #region game mecchanic

    private void Reset()
    {
        difficulty = 1;
        score = 0;
        flagPlayer = 0;
        flagEnemy = 0;
    }

    public void ReturnToMainMenu()
    {
        gameIsPaused = false;
        _menu.SetActive(gameIsPaused);
        Time.timeScale = 1f;
        Reset();
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        _menu.SetActive(gameIsPaused);
        Time.timeScale = gameIsPaused ? 0f : 1f;
    }
    void EndGame(bool isWin)
    {
        StartCoroutine(EndGameTimer(isWin));
    }

    IEnumerator EndGameTimer(bool isWin)
    {
        GameObject panel = isWin ? _gameWin : _gameOver;
        gameIsPaused = true;
        panel.SetActive(gameIsPaused);
        Time.timeScale = gameIsPaused ? 0f : 1f;
        yield return new WaitForSecondsRealtime(2f);
        gameIsPaused = false;
        panel.SetActive(gameIsPaused);
        Time.timeScale = gameIsPaused ? 0f : 1f;

#if UNITY_EDITOR
        String nextScene = "MainMenu";
#else
        int nextScene = 0;
#endif
        if (isWin)
        {
            _currentLevel++;
            if (_currentLevel < Levels.Count)
            {
#if UNITY_EDITOR
                nextScene = $"{Levels[_currentLevel].maze}";
#else
                nextScene = _currentLevel;
#endif
            }
        }
        Reset();
        SceneManager.LoadScene(nextScene);
    }

    #endregion

    public void IncrementFlagCount(bool isEnemy)
    {
        if (!isEnemy)
        {
            flagPlayer++;
            switch (difficulty)
            {
                case 0:
                    score += Levels[_currentLevel].flagCaptureBonus.easy;
                    break;
                case 1:
                    score += Levels[_currentLevel].flagCaptureBonus.medium;
                    break;
                case 2:
                    score += Levels[_currentLevel].flagCaptureBonus.hard;
                    break;
                default:
                    break;
            }

            gameIsEnded = flagPlayer == Levels[_currentLevel].flags;
        }
        else
        {
            flagEnemy++;
            //score += _flagEnemyValue;

            gameIsEnded = flagEnemy == Levels[_currentLevel].flags;
        }

        if (gameIsEnded) EndGame(flagPlayer == Levels[_currentLevel].flags);
    }

    public void IncrementScore(int value)
    {
        score += value;
    }

    public void PlayerSpeed(float value)
    {
        playerSpeed = value;
    }

}

[Serializable]
public class Level
{
    [Header("Level")]
    public string maze;

    [Header("Power-Up")]
    [Min(0)] public int powerUp;

    [Header("Object Counts")]
    [Min(0)] public int chaserBot;
    [Min(0)] public int scoutBot;
    [Min(0)] public int flags;

    [Header("Bonus by difficulty")]
    public Difficulty levelBonus;
    public Difficulty flagCaptureBonus;
    public Difficulty flagRemainBonus;
}

[Serializable]
public class Difficulty
{
    [Min(0)] public int easy;
    [Min(0)] public int medium;
    [Min(0)] public int hard;

    public Difficulty(int easy, int medium, int hard)
    {
        this.easy = easy;
        this.medium = medium;
        this.hard = hard;
    }
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(Difficulty))]
public class DifficultyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel = 0;

        var easy = property.FindPropertyRelative("easy");
        var medium = property.FindPropertyRelative("medium");
        var hard = property.FindPropertyRelative("hard");

        position.x += EditorGUIUtility.labelWidth;
        position.width -= EditorGUIUtility.labelWidth;

        float labelWidth = 30f;
        float spacing = 5f;

        EditorGUIUtility.labelWidth = labelWidth;

        position.width = (position.width - 2 * spacing) / 3f;

        EditorGUI.PropertyField(position, easy, new GUIContent("Easy"));
        position.x += position.width + spacing;

        EditorGUI.PropertyField(position, medium, new GUIContent("Medium"));
        position.x += position.width + spacing;

        EditorGUI.PropertyField(position, hard, new GUIContent("Hard"));

        EditorGUI.EndProperty();
    }
}

#endif