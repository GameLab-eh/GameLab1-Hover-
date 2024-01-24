using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioManager))]
public class AudioManager : MonoBehaviour
{
    [Header("FileAudio")]
    [SerializeField, Tooltip("Audio Source for...")] AudioSource _sample;
}
