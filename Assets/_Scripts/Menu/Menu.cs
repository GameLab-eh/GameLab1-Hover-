using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Button startButton;
    private void Awake()
    {
        startButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        startButton.onClick.AddListener(LoadScene);
    }
    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
