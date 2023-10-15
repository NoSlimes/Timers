using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NSJC.Timers
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { get; private set; }

        private List<Timer> timers = new();

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void LateUpdate()
        {
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Update();

                if (timers[i].IsFinished)
                {
                    if (timers[i].DestroyOnCompletion)
                    {
                        timers.RemoveAt(i);
                    }
                }
            }
        }

        public Timer CreateTimer(float durationInSeconds = 0, UnityAction onTimerFinished = null, bool startOnCreation = false, bool destroyOnCompletion = false, bool debugMode = false)
        {
            Timer timer = new(durationInSeconds, debugMode, destroyOnCompletion);
            timers.Add(timer);

            if (onTimerFinished != null)
            {
                timer.OnTimerFinished.AddListener(onTimerFinished);
            }

            if (startOnCreation)
            {
                timer.Play();
            }
            return timer;
        }

        public void PlayTimer(Timer timer)
        {
            timer.Play();
        }

        public void PauseTimer(Timer timer)
        {
            timer.Pause();
        }

        public void SetTimerDuration(Timer timer, float duration)
        {
            timer.SetDuration(duration);
        }

        public void ResetTimer(Timer timer, bool pause = true)
        {
            timer.Reset(pause);
        }

        public Timer DestroyTimer(Timer timer)
        {
            if (timer.DebugMode)
            {
                Debug.Log($"Destroying Timer");
            }

            timers.Remove(timer);

            return null;
        }
    }
}