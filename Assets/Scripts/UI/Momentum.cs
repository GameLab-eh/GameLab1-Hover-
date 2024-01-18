using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Momentum : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] GameObject _momentun;
    [SerializeField] GameObject _player;
    public Vector3 direzioneDesiderata = Vector3.forward;

    void Update()
    {
        if (_player.transform == null || _momentun == null)
        {
            Debug.LogWarning("Assegna tutti i riferimenti nell'Inspector.");
            return;
        }

        direzioneDesiderata.Normalize();

        // Crea la rotazione in base alla direzione desiderata
        Quaternion rotazioneDesiderata = Quaternion.LookRotation(direzioneDesiderata);

        // Applica la rotazione all'oggetto
        _momentun.transform.rotation = rotazioneDesiderata;
    }
}
