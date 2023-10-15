using UnityEngine;
using UnityEngine.Events;

namespace NSJC.Timers
{
    public class Timer
    {
        public bool IsRunning { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsPaused { get; private set; }

        public float InitialDuration { get; private set; }
        public float RemainingDuration { get; private set; }

        public bool DestroyOnCompletion { get; private set; }

        public bool DebugMode { get; private set; }

        public UnityEvent OnTimerStarted { get; private set; } = new UnityEvent();
        public UnityEvent OnTimerFinished { get; private set; } = new UnityEvent();

        public Timer(float duration, bool debugMode, bool destroyOnCompletion)
        {
            InitialDuration = duration;
            RemainingDuration = duration;
            DebugMode = debugMode;
            DestroyOnCompletion = destroyOnCompletion;
        }

        public void Play()
        {
            IsRunning = true;
            IsPaused = false;

            if (DebugMode)
            {
                UnityEngine.Debug.Log($"Playing timer, remaining phaseDuration {RemainingDuration}");
            }
        }

        public void Pause()
        {
            IsRunning = false;
            IsPaused = true;

            if (DebugMode)
            {
                UnityEngine.Debug.Log($"Paused timer, remaining phaseDuration {RemainingDuration}");
            }
        }

        public void Stop()
        {
            IsFinished = true;

            if (DebugMode)
            {
                UnityEngine.Debug.Log($"Stopped timer, remaining phaseDuration at stop {RemainingDuration}");
            }
        }

        public void SetDuration(float duration)
        {
            InitialDuration = duration;
            RemainingDuration = duration;

            IsFinished = false;

            if (DebugMode)
            {
                UnityEngine.Debug.Log($"Set timer phaseDuration to {duration}");
            }
        }

        public void Reset(bool pause = true)
        {
            RemainingDuration = InitialDuration;
            IsFinished = false;

            if (!IsPaused)
            {
                Play();
            }

            if (DebugMode)
            {
                UnityEngine.Debug.Log($"Reset timer with phaseDuration: {InitialDuration}, paused: {pause}");
            }
        }

        public void Update()
        {
            if (IsRunning && !IsPaused)
            {
                RemainingDuration -= Time.deltaTime;

                if (RemainingDuration <= 0)
                {
                    IsRunning = false;
                    IsFinished = true;

                    OnTimerFinished?.Invoke();

                    if (DebugMode)
                    {
                        UnityEngine.Debug.Log($"Timer Finished!");
                    }
                }
            }
        }

    }
}