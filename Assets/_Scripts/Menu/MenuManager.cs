using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button _play;
    [SerializeField] Button _option;
    [SerializeField] Button _quit;

    [Header("Option Panel")]
    [SerializeField] CanvasGroup _panel;
    [SerializeField, Min(0)] float _duration;
    bool _isVisible = true;

    bool _checkOption;

    private void OnEnable()
    {
        _play.onClick.AddListener(Play);
        _option.onClick.AddListener(Option);
        _quit.onClick.AddListener(Quit);
    }

    private void OnDisable()
    {
        _play?.onClick.RemoveListener(Play);
        _option.onClick.RemoveListener(Option);
        _quit?.onClick.RemoveListener(Quit);
    }

    public void Play()
    {
#if UNITY_EDITOR
        SceneManager.LoadScene("Build 0.3");
#else
        SceneManager.LoadScene(1);
#endif
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
            elapsedTime += Time.deltaTime;
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
