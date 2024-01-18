using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsInventory : MonoBehaviour
{
    [Header("Indicator")]
    [SerializeField] List<Slider> _indicator;
    [SerializeField] List<TMPro.TMP_Text> _numberDisplay;
    [SerializeField] List<TMPro.TMP_Text> _buttonDisplay;

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

    private void Start()
    {
        _inputSystem = GameManager.Instance.GetInputSystem();
        SetInputSystemDisplay();
        _invisibilityDuration = GameManager.Instance.GetInvisibilityDuration();
        _wallDestroyDelay = GameManager.Instance.GetWallDelayDestroy();
    }

    void Update()
    {
        _indicator[0].value = Mathf.Clamp((Mathf.Round(((_player.transform.position.y / _maxAltitude) * 100f) * 10f) / 10f), 0f, 100f) / 100f;
    }

    public void OnEnable()
    {
        PlayerController.Stack1 += SetStack1; //jump
        PlayerController.Stack2 += SetStack2; //wall
        PlayerController.Stack3 += SetStack3; //invisibility
        PlayerController.StackUse += SetSlider;
    }
    public void OnDisable()
    {
        PlayerController.Stack1 -= SetStack1; //jump
        PlayerController.Stack2 -= SetStack2; //wall
        PlayerController.Stack3 -= SetStack3; //invisibility
        PlayerController.StackUse -= SetSlider;
    }

    void SetStack1(int value) => _numberDisplay[0].text = "" + value;
    void SetStack2(int value)
    {
        _numberDisplay[1].text = "" + value;
    }

    void SetStack3(int value)
    {
        _numberDisplay[2].text = "" + value;
    }
    void SetSlider(int value)
    {
        if (value == 1) StartCoroutine(DecrementSlider(1, _wallDestroyDelay));
        else StartCoroutine(DecrementSlider(2, _invisibilityDuration));
    }

    private IEnumerator DecrementSlider(int value, float second)
    {
        float initialPercentage = 1000f;

        _indicator[value].value = initialPercentage;

        while (initialPercentage > 0f)
        {
            initialPercentage--;
            _indicator[value].value = (float)initialPercentage / 1000;

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
