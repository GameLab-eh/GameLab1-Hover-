using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class LoadSceneManager : MonoBehaviour
{
    void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
