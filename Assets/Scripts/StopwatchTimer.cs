using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Helper object used to manage the timing and polling for chromosome evaluations.
/// Implementation is inspired by a stopwatch.
/// </summary>
public class StopwatchTimer
{
    public UnityEvent OnTimerPoll;                      // Event used for notifying listeners of when a specified duration has elapsed in the timer.          
    private float DurationBeforePoll, InitialTimeStamp; // Specified duration before polling and time stamp used for calculating durations respectively.
    private bool IsActive;                              // Boolean indicating whether the timer is currently active.

    /// <summary>
    /// Constructor. Initialises event and duration for the polling mechanism.
    /// </summary>
    /// <param name="durationBeforePoll">Duration before poll action is performed</param>
    public StopwatchTimer(float durationBeforePoll)
    {
        OnTimerPoll = new UnityEvent();
        DurationBeforePoll = durationBeforePoll;
    }

    public void Update()
    {
        if (IsActive)
        {
            // Has specified duration elapsed?
            float duration = Time.time - InitialTimeStamp;
            if (duration >= DurationBeforePoll)
            {
                // Disable timer and perform poll action
                Pause();
                OnTimerPoll.Invoke();
            }
        }
    }

    /// <summary>
    /// Utility method for enabling the timer.
    /// </summary>
    public void Play()
    {
        IsActive = true;
    }

    /// <summary>
    /// Utility method for disabling the timer.
    /// </summary>
    public void Pause()
    {
        IsActive = false;
    }

    /// <summary>
    /// Utility method for resetting the time stamp for the timer
    /// and enabling the timer.
    /// </summary>
    public void Reset()
    {
        InitialTimeStamp = Time.time;
        Play();
    }
}
