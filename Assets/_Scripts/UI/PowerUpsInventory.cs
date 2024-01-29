using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsInventory : MonoBehaviour
{
    [Header("Indicator")]
    [SerializeField] List<Slider> _indicator;
    [SerializeField] List<TMPro.TMP_Text> _numberDisplay;
    [SerializeField] List<TMPro.TMP_Text> _buttonDisplay;
    [Header("Bottom-Left IU Area")]
    [SerializeField] Slider _indicatorShield;
    [SerializeField] List<Slider> _indicatorStoplight;

    [Header("Altitude")]
    [SerializeField] GameObject _player;
    [SerializeField] Vector3 _ground;
    [SerializeField, Range(0f, 20f)] float _maxAltitude;
    [SerializeField] float _Altitude;

    [Header("Input")]
    [SerializeField] string _opz1;
    [SerializeField] string _opz1alt;
    [SerializeField] string _opz2;
    [SerializeField] string _opz2alt;
    [SerializeField] string _opz3;
    [SerializeField] string _opz3alt;

    bool _inputSystem;
    float _invisibilityDuration;
    float _wallDestroyDelay;
    float _shieldDuration;
    float _stoplightDuration;

    private Coroutine currentCoroutine;

    private void Start()
    {
        _inputSystem = GameManager.Instance.GetInputSystem();
        SetInputSystemDisplay();
        _invisibilityDuration = GameManager.Instance.GetInvisibilityDuration();
        _wallDestroyDelay = GameManager.Instance.GetWallDelayDestroy();
        _shieldDuration = GameManager.Instance.GetShieldDuration();
        _stoplightDuration = GameManager.Instance.GetStoplightDuration();
    }

    void Update()
    {
        _indicator[0].value = Mathf.Clamp((Mathf.Round(((_player.transform.position.y / _maxAltitude) * 100f) * 10f) / 10f), 0f, 100f) / 100f;
        if (_inputSystem == GameManager.Instance.GetInputSystem()) SetInputSystemDisplay();
        _inputSystem = GameManager.Instance.GetInputSystem();
    }

    public void OnEnable()
    {
        PlayerController.Stack += SetStack;
        PlayerController.StackUse += SetSlider;
        PlayerController.Shield += SetShieldSlider; //shield
        PlayerController.Stoplight += SetStoplightSlider; //stoplight
    }
    public void OnDisable()
    {
        PlayerController.Stack -= SetStack;
        PlayerController.StackUse -= SetSlider;
        PlayerController.Shield -= SetShieldSlider; //shield
        PlayerController.Stoplight -= SetStoplightSlider; //stoplight
    }

    void SetShieldSlider() => StartCoroutine(DecrementSlider(_indicatorShield, _shieldDuration));

    void SetStoplightSlider(bool value)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        _indicatorStoplight[0].value = 0f;
        _indicatorStoplight[1].value = 0f;

        if (!value) currentCoroutine = StartCoroutine(DecrementSlider(_indicatorStoplight[0], _stoplightDuration));
        else currentCoroutine = StartCoroutine(DecrementSlider(_indicatorStoplight[1], _stoplightDuration));
    }

    void SetStack(int t, int value) => _numberDisplay[t].text = "" + value;

    void SetSlider(int t, int value)
    {
        _numberDisplay[t].text = "" + value;
        StartCoroutine(DecrementSlider(_indicator[t], _wallDestroyDelay));
    }

    private IEnumerator DecrementSlider(Slider slider, float second)
    {
        float initialPercentage = 1000f;

        slider.value = initialPercentage;

        while (initialPercentage > 0f)
        {
            initialPercentage--;
            slider.value = (float)initialPercentage / 1000;

            yield return new WaitForSeconds(second / 1750);
        }
    }

    void SetInputSystemDisplay()
    {
        if (_inputSystem)
        {
            _buttonDisplay[0].text = _opz1;
            _buttonDisplay[1].text = _opz2;
            _buttonDisplay[2].text = _opz3;
        }
        else
        {
            _buttonDisplay[0].text = _opz1alt;
            _buttonDisplay[1].text = _opz2alt;
            _buttonDisplay[2].text = _opz3alt;
        }
    }
}
