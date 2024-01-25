using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Game menu")]
    [SerializeField] GameObject gameMenu;

    private void Awake()
    {
        GameManager.Instance.SetGameMenu(gameMenu.gameObject);
    }
}
