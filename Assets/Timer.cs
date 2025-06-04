using UnityEngine;
using System;
using System.Collections;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public string timerName = "Timer";
    public bool loop = false;
    public float duration = 5f;

    public event Action OnTimerCompleted;

    private Coroutine timerCoroutine;
    private float endTime = 0f;
    private bool running = false;

    public void StartTimer()
    {
        // Ogni volta che parte, il timer si resetta e va da duration a 0
        StopTimer();
        running = true;
        endTime = Time.realtimeSinceStartup + duration;
        timerCoroutine = StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        running = false;
    }

    public void ResetTimer()
    {
        StopTimer();
        endTime = 0f;
    }

    public float RemainingTime()
    {
        // Restituisce sempre un valore che va da duration a 0
        if (!running) return duration;
        float remaining = endTime - Time.realtimeSinceStartup;
        return Mathf.Max(0f, remaining);
    }

    public float ElapsedTime()
    {
        if (!running) return 0f;
        float elapsed = duration - RemainingTime();
        return Mathf.Clamp(elapsed, 0f, duration);
    }

    public bool IsRunning()
    {
        return running;
    }

    private IEnumerator TimerRoutine()
    {
        while (running)
        {
            float waitTime = endTime - Time.realtimeSinceStartup;
            if (waitTime > 0f)
                yield return new WaitForSecondsRealtime(waitTime);
            else
                yield return null;

            if (!running) yield break;

            OnTimerCompleted?.Invoke();

            if (loop)
            {
                endTime = Time.realtimeSinceStartup + duration;
            }
            else
            {
                running = false;
            }
        }
    }
}