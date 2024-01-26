using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EasterEgg : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] TMP_InputField _inputField;
    [SerializeField, Tooltip("Password for open EasterEgg Scene")] string _password;

    private void OnEnable()
    {
        _inputField.onEndEdit.AddListener(CheckInputField);
    }

    private void OnDisable()
    {
        _inputField.onEndEdit.RemoveListener(CheckInputField);
    }

    public void CheckInputField(string value)
    {
        if (value == _password)
        {
#if UNITY_EDITOR
            SceneManager.LoadScene("EasterEgg");
#else
            SceneManager.LoadScene(GameManager.Instance.GetLevel());
#endif
            GameManager.Instance.SetCurrentLevel(999);
        }
    }
}
