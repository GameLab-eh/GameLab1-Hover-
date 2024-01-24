using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimerCustom
{
    public class CustomTimer
    {
        private float Time;
        private float currentTime;

        private Action _action;

        public CustomTimer(Action action, float time)
        {
            _action = action;
            Time = time;
            currentTime = 0;
        }
        public void Tick(float deltaTime)
        {
            currentTime += deltaTime;
            if (currentTime >= Time)
            {
                _action.Invoke();
                currentTime = 0;
            }
        }
    }

    /*Esempio temporizzatore che stampa un messaggio dopo 3 secondi
    
    CustomTimer myTimer = new CustomTimer(() => { Debug.Log("Temporizzatore scaduto!"); }, 3f);
    //Aggiungere il temporizzatore al TimerManager
    TimerManager.AddTimer(myTimer);
    
    */

}