using UnityEngine.Events;
using UnityEngine;

public class CooldownTimer
{
    public UnityEvent OnCooldownPoll;
    private float DurationBeforePoll, InitialTimeStamp;
    private bool IsActive;

    public CooldownTimer(float durationBeforePoll)
    {
        OnCooldownPoll = new UnityEvent();
        DurationBeforePoll = durationBeforePoll;
    }

    public void Update()
    {
        if (IsActive)
        {
            float duration = Time.time - InitialTimeStamp;
            if (duration >= DurationBeforePoll)
            {
                Pause();
                OnCooldownPoll.Invoke();
            }
        }
    }

    public void Play()
    {
        IsActive = true;
    }

    public void Pause()
    {
        IsActive = false;
    }

    public void Reset()
    {
        InitialTimeStamp = Time.time;
        Play();
    }
}
