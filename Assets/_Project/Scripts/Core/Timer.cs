using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mystie.Core
{
    [Serializable]
    public class Timer
    {
        public Action onTimerEnd;

        private float duration;
        public float time { get; private set; }

        public Timer(float _time = 0)
        {
            SetTime(_time);
        }

        public void SetTime(float _time)
        {
            time = _time;
        }

        public void Tick(float deltaTime)
        {
            if (time <= 0f && deltaTime >= 0) return;

            time -= deltaTime;
            CheckForTimerEnd();
        }

        public void CheckForTimerEnd()
        {
            if (time > 0f) return;
            time = 0f;

            onTimerEnd?.Invoke();
        }

        public bool IsRunning => time > 0f;
    }
}
