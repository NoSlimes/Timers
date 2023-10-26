using System;
using UnityEngine;
using UnityEngine.Events;

namespace NSJC.Timers
{
    public class Timer
    {
        /// <summary>
        /// Gets a value indicating whether the timer is currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer has finished.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer has been canceled. 
        /// </summary>
        public bool IsCanceled { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer is currently paused.
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer is set to repeating.
        /// </summary>
        public bool IsRepeating { get; private set; }

        /// <summary>
        /// Gets the initial duration set for the timer.
        /// </summary>
        public float InitialDuration { get; private set; }

        /// <summary>
        /// Gets the remaining duration of the timer.
        /// </summary>
        public float RemainingDuration { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer should be destroyed upon completion.
        /// </summary>
        public bool DestroyOnCompletion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the timer is in debug mode for logging.
        /// </summary>
        public bool DebugMode { get; private set; }

        /// <summary>
        /// Event that is triggered when the timer is started.
        /// </summary>
        public UnityEvent OnTimerStarted { get; private set; } = new();

		/// <summary>
		/// Event that is triggered when the timer is finished.
		/// </summary>
		public UnityEvent OnTimerFinished { get; private set; } = new();

        /// <summary>
        /// Event that is triggered when the timer is canceled.
        /// </summary>
        public UnityEvent<float> OnTimerCanceled { get; private set; } = new();

        /// <summary>
        /// Event that is triggered every timer update.
        /// </summary>
        public UnityEvent<float> OnTimerUpdate { get; private set; } = new();

        /// <summary>
        /// Initializes a new instance of the Timer class with the specified duration, debug mode, and destroyOnCompletion setting.
        /// </summary>
        /// <param name="duration">The initial duration of the timer.</param>
        /// <param name="isRepeating">Whether to enable repeat on the timer.</param>
        /// <param name="destroyOnCompletion">Whether to destroy the timer on completion.</param>
        /// <param name="debugMode">Whether to enable debug mode for logging.</param>
        public Timer(float duration, bool isRepeating, bool destroyOnCompletion, bool debugMode)
        {
            InitialDuration = duration;
            RemainingDuration = duration;
            IsRepeating = isRepeating;
            DestroyOnCompletion = destroyOnCompletion;
            DebugMode = debugMode;
        }

        /// <summary>
        /// Starts the timer, making it run.
        /// </summary>
        public void Play()
        {
            IsRunning = true;
            IsPaused = false;

            if (DebugMode)
            {
                Debug.Log($"Playing timer, remaining newDuration: {RemainingDuration}");
            }
        }

        /// <summary>
        /// Pauses the timer, stopping it temporarily.
        /// </summary>
        public void Pause()
        {
            IsRunning = false;
            IsPaused = true;

            if (DebugMode)
            {
                Debug.Log($"Paused timer, remaining newDuration: {RemainingDuration}");
            }
        }

        /// <summary>
        /// Cancels the timer and marks it as canceled.
        /// </summary>
        public void Cancel()
        {
            IsCanceled = true;

            if (DebugMode)
            {
                Debug.Log($"Canceled timer, remaining newDuration at stop: {RemainingDuration}");
            }

            OnTimerCanceled?.Invoke(RemainingDuration);
        }

        /// <summary>
        /// Sets the duration of the timer and optionally pauses it.
        /// </summary>
        /// <param name="newDuration">The new duration to set for the timer.</param>
        /// <param name="playOnDurationSet">Whether to play the timer when setting the duration.</param>
        public void SetDuration(float newDuration, bool playOnDurationSet = false)
        {
            if (playOnDurationSet)
            {
                Play();
            }
            else
            {
                Pause();
            }

            InitialDuration = newDuration;
            RemainingDuration = newDuration;

            IsFinished = false;
            IsCanceled = false;

            if (DebugMode)
            {
                Debug.Log($"Set timer newDuration to: {newDuration}, playOnReset: {playOnDurationSet}");
            }
        }

        /// <summary>
        /// Resets the timer to its initial duration and optionally resumes it from a paused state.
        /// </summary>
        /// <param name="playOnReset">Whether to play the timer when resetting.</param>
        public void Reset(bool playOnReset = false)
        {
            RemainingDuration = InitialDuration;
            IsFinished = false;
            IsCanceled = false;

            if (playOnReset)
            {
                Play();
            }

            if (DebugMode)
            {
                Debug.Log($"Reset timer with newDuration: {InitialDuration}, playOnReset: {playOnReset}");
            }
        }

        /// <summary>
        /// Update method that should be called in the game loop to update the timer's state.
        /// </summary>
        internal void Update()
        {
            if (IsRunning && !IsPaused && !IsCanceled)
            {
                RemainingDuration -= Time.deltaTime;

                if (RemainingDuration <= 0)
                {
                    IsRunning = false;
                    IsFinished = true;

                    OnTimerFinished?.Invoke();

                    if (DebugMode)
                    {
                        Debug.Log($"Timer Finished!");
                    }

                    if (IsRepeating)
                    {
                        Reset(playOnReset: true);
                    }

                }

                OnTimerUpdate?.Invoke(RemainingDuration);
            }
        }
    }
}
