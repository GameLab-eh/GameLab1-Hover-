using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TimerCustom;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private static List<CustomTimer> Timers;

    private void Awake()
    {
        Timers = new List<CustomTimer>();
    }
    private void Update()
    {
        for (int i = 0; i < Timers.Count; i++)
        {
            Timers[i].Tick(Time.deltaTime);
        }
    }
    public static void AddTimer(CustomTimer timer)
    {
        Timers.Add(timer);
    }
}
