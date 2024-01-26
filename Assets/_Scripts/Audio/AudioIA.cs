using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioIA : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] AudioClip _audioClip;

    [Header("Variables")]
    [SerializeField, Min(0)] float _spatialBlend = 1f;

    [SerializeField, Min(0)] float _delay = 2f;

    AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = _audioClip;

        audioSource.spatialBlend = _spatialBlend;
        float delay = _audioClip.length - 2f; 
        StartCoroutine(PlayDelayedAudio(delay, _delay));
    }

    private void FixedUpdate()
    {
        audioSource.volume = GameManager.Instance.AudioManager.IAVolumeMaster() * GameManager.Instance.AudioManager.IAVolumeEffects(); 
    }

    IEnumerator PlayDelayedAudio(float initialDelay, float repeatRate)
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            audioSource.Play();
            yield return new WaitForSeconds(repeatRate);
        }
    }
}
