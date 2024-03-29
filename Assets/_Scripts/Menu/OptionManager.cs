using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Windows;

public class OptionManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] Slider _difficulty;
    [SerializeField] Slider _input;
    [SerializeField] Slider _master;
    [SerializeField] Slider _music;
    [SerializeField] Slider _effects;

    public delegate void OptionManagerInt(int value);
    public static event OptionManagerInt Difficulty = null;

    public delegate void OptionManagerFloat(float value);
    public static event OptionManagerFloat Master = null;
    public static event OptionManagerFloat Music = null;
    public static event OptionManagerFloat Effects = null;

    public delegate void OptionManagerBool(bool value);
    public static event OptionManagerBool Input = null;

    private void OnEnable()
    {
        if (_difficulty != null) _difficulty.onValueChanged.AddListener(SliderDifficulty);
        _input.onValueChanged.AddListener(SliderInputSystem);
        _master.onValueChanged.AddListener(SliderMaster);
        _music.onValueChanged.AddListener(SliderMusic);
        _effects.onValueChanged.AddListener(SliderEffects);

        AudioManager.VolumeChanged += SliderUpdate;
    }

    private void OnDisable()
    {
        if (_difficulty != null) _difficulty.onValueChanged.RemoveListener(SliderDifficulty);
        _input.onValueChanged.RemoveListener(SliderInputSystem);
        _master.onValueChanged.RemoveListener(SliderMaster);
        _music.onValueChanged.RemoveListener(SliderMusic);
        _effects.onValueChanged.RemoveListener(SliderEffects);
    }

    void OnAlphaChanged(float alpha) { }

    public void SliderDifficulty(float value) => Difficulty?.Invoke((int)value);

    public void SliderInputSystem(float value) => Input?.Invoke((int)value > 0);

    public void SliderMaster(float value) => Master?.Invoke(value);

    public void SliderMusic(float value) => Music?.Invoke(value);

    public void SliderEffects(float value) => Effects?.Invoke(value);

    void SliderUpdate(float master, float music, float effects)
    {
        _master.value = master;
        _music.value = music;
        _effects.value = effects;
    }
}
