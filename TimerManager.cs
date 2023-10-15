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

        /// <summary>
        /// Creates a new Timer and adds it to the list of managed timers.
        /// </summary>
        /// <param name="durationInSeconds">The initial duration of the timer in seconds.</param>
        /// <param name="onTimerFinished">An optional UnityAction to be executed when the timer finishes.</param>
        /// <param name="startOnCreation">Indicates whether the timer should start immediately upon creation.</param>
        /// <param name="destroyOnCompletion">Indicates whether the timer should be destroyed upon completion.</param>
        /// <param name="debugMode">Enables or disables debug mode for the timer.</param>
        /// <returns>The created Timer object.</returns>
        public Timer CreateTimer(float durationInSeconds = 0, UnityAction onTimerFinished = null, bool startOnCreation = false, bool destroyOnCompletion = false, bool debugMode = false)
        {
            Timer timer = new Timer(durationInSeconds, debugMode, destroyOnCompletion);
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

        /// <summary>
        /// Starts a Timer that is currently paused.
        /// </summary>
        /// <param name="timer">The Timer to be played.</param>
        public void PlayTimer(Timer timer)
        {
            timer.Play();
        }

        /// <summary>
        /// Pauses a running Timer.
        /// </summary>
        /// <param name="timer">The Timer to be paused.</param>
        public void PauseTimer(Timer timer)
        {
            timer.Pause();
        }

        /// <summary>
        /// Sets the duration of a Timer, optionally pausing it in the process.
        /// </summary>
        /// <param name="timer">The Timer to be modified.</param>
        /// <param name="duration">The new duration for the Timer.</param>
        public void SetTimerDuration(Timer timer, float duration)
        {
            timer.SetDuration(duration);
        }

        /// <summary>
        /// Resets a Timer to its initial duration, optionally resuming it from a paused state.
        /// </summary>
        /// <param name="timer">The Timer to be reset.</param>
        /// <param name="pause">Indicates whether the Timer should be paused after the reset.</param>
        public void ResetTimer(Timer timer, bool pause = true)
        {
            timer.Reset(pause);
        }

        /// <summary>
        /// Destroys and removes a Timer from the list of managed timers.
        /// </summary>
        /// <param name="timer">The Timer to be destroyed.</param>
        /// <returns>Returns null after the Timer is destroyed.</returns>
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