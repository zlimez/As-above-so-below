using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Chronellium.TimeManagers
{
    /// <summary>
    /// A Timer class that provides functionality for starting, stopping, resetting, and modifying a timer in a Unity game.
    /// </summary>
    public class CountdownTimer : MonoBehaviour
    {
        public float TimeLeft;

        /// <summary>
        /// Total duration of the timer in seconds.
        /// </summary>
        [SerializeField] public float TotalDuration = 30;

        [System.NonSerialized] public UnityEvent<float> OnTimerChange = new UnityEvent<float>();
        [System.NonSerialized] public UnityEvent OnTimerStart = new UnityEvent();
        [System.NonSerialized] public UnityEvent OnTimerExpire = new UnityEvent();

        private IEnumerator _timerCoroutine;

        /// <summary>
        /// Called when the timer component is enabled.
        /// </summary>
        private void OnEnable()
        {
            Debug.Log("Timer Enable");
            TimeLeft = TotalDuration;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer()
        {
            OnTimerStart.Invoke();
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }
            _timerCoroutine = TimerCoroutine();
            StartCoroutine(_timerCoroutine);
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
            OnTimerChange.RemoveAllListeners();
        }

        /// <summary>
        /// Resets the timer to its initial duration.
        /// </summary>
        public void ResetTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }
            TimeLeft = TotalDuration;
            OnTimerChange.Invoke(TimeLeft);
        }

        /// <summary>
        /// Adds time to the timer.
        /// </summary>
        /// <param name="timeToAdd">Amount of time in seconds to add to the timer.</param>
        public void AddTime(int timeToAdd)
        {
            TimeLeft += timeToAdd;
            if (TimeLeft > TotalDuration)
            {
                TimeLeft = TotalDuration;
            }
            OnTimerChange.Invoke(TimeLeft);
        }

        /// <summary>
        /// Coroutine that handles the timer countdown.
        /// </summary>
        /// <returns>An IEnumerator to be used in a coroutine.</returns>
        private IEnumerator TimerCoroutine()
        {
            while (TimeLeft > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                TimeLeft -= Time.deltaTime;
                OnTimerChange.Invoke(TimeLeft);

                if (TimeLeft <= 0)
                {
                    Debug.Log("Timer Expire");
                    OnTimerExpire.Invoke();
                }
            }
        }
    }

}