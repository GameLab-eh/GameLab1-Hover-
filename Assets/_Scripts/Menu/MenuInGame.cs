using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button _resume;
    [SerializeField] Button _option;
    [SerializeField] Button _quitMenu;
    [SerializeField] Button _quit;

    [Header("Option Panel")]
    [SerializeField] CanvasGroup _panel;
    [SerializeField, Min(0)] float _duration = 0.5f;
    bool _isVisible = true;

    bool _checkOption;

    private void OnEnable()
    {
        _resume.onClick.AddListener(Resume);
        _option.onClick.AddListener(Option);
        _quitMenu.onClick.AddListener(QuitMenu);
        _quit.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        _resume?.onClick.RemoveListener(Resume);
        _option.onClick.RemoveListener(Option);
        _quitMenu.onClick.AddListener(QuitMenu);
        _quit?.onClick.RemoveListener(Quit);
    }

    public void Resume()
    {
        this.gameObject.SetActive(false);
        GameManager.Instance.PauseGame();
    }

    public void QuitMenu()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene("MainMenu");
#else
        SceneManager.LoadScene(0);
#endif
        GameManager.Instance.Resume();
    }

    public void Option()
    {
        if (!_checkOption)
        {
            StartCoroutine(OptionCoroutine());
        }
    }

    IEnumerator OptionCoroutine()
    {
        _checkOption = true;

        _isVisible = !_isVisible;

        float startAlpha = _isVisible ? 1f : 0f;
        float targetAlpha = _isVisible ? 0f : 1f;
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            _panel.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _duration);
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }

        _panel.alpha = targetAlpha;
        _checkOption = false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
