using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static OptionManager;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(AudioManager))]
public class AudioManager : MonoBehaviour
{
    [Header("Soundtrack")]
    [SerializeField] AudioClip _soundtrack;

    [Header("Audio Effects")]
    [SerializeField] List<SoundTrack> _tracks;

    [Header("Audio Set")]
    [SerializeField, Range(0, 1)] float _volumeMaster = 0.5f;
    [SerializeField, Range(0, 1)] float _voluemMusic = 0.5f;
    [SerializeField, Range(0, 1)] float _volumeEffects = 0.5f;

    [Header("Variables")]
    [SerializeField, Range(0, 1)] float _advance = 0.2f;

    AudioSource _music;
    AudioSource _effect;

    public delegate void AudioManagerVolumeChanged(float master, float music, float effects);
    public static event AudioManagerVolumeChanged VolumeChanged;
    private void Start()
    {
        _music = gameObject.AddComponent<AudioSource>();
        _effect = gameObject.AddComponent<AudioSource>();

        PlaySoundtrack();
    }

    private void OnEnable()
    {
        OptionManager.master += ChangeVolumeMaster;
        OptionManager.music += ChangeVolumeMusic;
        OptionManager.effects += ChangeVolumeEffects;
    }

    private void OnDisable()
    {
        OptionManager.master -= ChangeVolumeMaster;
        OptionManager.music -= ChangeVolumeMusic;
        OptionManager.effects -= ChangeVolumeEffects;
    }

    void PlaySoundtrack()
    {
        _music.clip = _soundtrack;
        _music.loop = true;
        _music.volume = _volumeMaster * _voluemMusic;
        _music.Play();
    }

    public void PlayEffect(string index)
    {
        foreach (SoundTrack track in _tracks)
        {
            if (track.trackName == index)
            {
                //_effect.clip = track.track;
                //_effect.time = _advance;
                //_music.volume = _volumeMaster * _volumeEffects;
                _effect.PlayOneShot(track.track);
            }
        }
    }

    void ChangeVolumeMaster(float volume)
    {
        _volumeMaster = Mathf.Clamp01(volume);
        UpdateVolume();
    }
    void ChangeVolumeMusic(float volume)
    {
        _voluemMusic = Mathf.Clamp01(volume);
        UpdateVolume();
    }

    void ChangeVolumeEffects(float volume)
    {
        _volumeEffects = Mathf.Clamp01(volume);
        UpdateVolume();
    }

    public float IAVolumeMaster() { return _volumeMaster; }
    public float IAVolumeEffects() { return _volumeEffects; }

    void UpdateVolume()
    {
        _music.volume = _volumeMaster * _voluemMusic;
        _effect.volume = _volumeMaster * _volumeEffects;

        VolumeChanged?.Invoke(_volumeMaster, _voluemMusic, _volumeEffects);
    }
}

[Serializable]
public class SoundTrack
{
    [Header("Track name")]
    public string trackName;

    [Header("Sound Track")]
    public AudioClip track;
}