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
        /// <param name="isRepeating">An optional bool to set whether the timer should be repeating or not.</param>
        /// <param name="startOnCreation">An optional bool to set whether the timer should start immediately upon creation.</param>
        /// <param name="destroyOnCompletion">An optional bool to set whether the timer should be destroyed upon completion.</param>
        /// <param name="debugMode">An optional bool that enables or disables debug mode for the timer.</param>
        /// <returns>The created Timer object.</returns>
        public Timer CreateTimer(float durationInSeconds = 0, UnityAction onTimerFinished = null, bool isRepeating = false, bool startOnCreation = false, bool destroyOnCompletion = false, bool debugMode = false)
        {
            Timer timer = new(durationInSeconds, isRepeating, debugMode, destroyOnCompletion);
            timers.Add(timer);

            if (onTimerFinished != null)
            {
                timer.OnTimerFinished.AddListener(onTimerFinished);
            }

            if (startOnCreation)
            {
                timer.Play();
            }

            if (debugMode)
            {
                Debug.Log($"Created timer {timer}, with a duration of {durationInSeconds} seconds. isRepeating: {isRepeating}. startOnCreation{startOnCreation}. destroyOnCompletion: {destroyOnCompletion}. debugMode: {debugMode}");
            }

            return timer;
        }

        /// <summary>
        /// Plays all Timers that is currently paused.
        /// </summary>
        public void PlayAllTimers()
        {
            foreach(Timer timer in timers)
            {
                timer.Play();
            }
        }

        /// <summary>
        /// Pauses all running Timers.
        /// </summary>
        public void PauseAllTimers()
        {
            foreach (Timer timer in timers)
            {
                timer.Pause(); 
            }
        }

        /// <summary>
        /// Cancels all Timers.
        /// </summary>
        public void CancelAllTimers()
        {
            foreach (Timer timer in timers)
            {
                timer.Cancel();
            }
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