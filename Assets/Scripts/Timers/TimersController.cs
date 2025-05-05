using System.Collections.Generic;
using UnityEngine;

public class TimersController : MonoBehaviour
{
    private static TimersController instance;

    [SerializeField] private int amount;

    public readonly List<Timer> timersList = new();

    private Timer currentTimer;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        amount = timersList.Count;
        UpdateTimers();
    }

    public void AddTimer(Timer timer)
    {
        timersList.Add(timer);
    }

    public void RemoveTimer(Timer timer)
    {
        timer.OnStart().RemoveAllListeners();
        timer.OnValueChanged().RemoveAllListeners();
        timer.OnEnd().RemoveAllListeners();
        timersList.Remove(timer);
    }

    private void UpdateTimers()
    {
        for (var i = timersList.Count - 1; i >= 0; i--)
        {
            currentTimer = timersList[i];
            UpdateTimer(currentTimer);
        }
    }

    private void UpdateTimer(Timer timer)
    {
        if (timer.ToDelete())
        {
            RemoveTimer(timer);
        }
        else if (timer.IsActive())
        {
            timer.SetValue(timer.GetValue() - Time.deltaTime);
            timer.UpdateDelta();
            if (timer.GetValue() <= 0f)
            {
                timer.OnEnd().Invoke();
                if (timer.IsContinuous())
                    timer.Restart();
                else
                    timer.ResetTimer();
            }
            else
            {
                timer.OnValueChanged().Invoke();
            }
        }
    }

    public static TimersController GetController()
    {
        return instance;
    }
}