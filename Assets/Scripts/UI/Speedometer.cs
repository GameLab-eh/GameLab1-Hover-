using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] Slider _slide;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _slide.value = GameManager.Instance.GetPlayerSpeed() / 100f;
    }
}
