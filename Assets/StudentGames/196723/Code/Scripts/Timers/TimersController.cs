using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TimersController : MonoBehaviour
{
    [SerializeField]
    private int amount;
    public readonly List<Timer> timersList = new List<Timer>();
    private static TimersController instance = null;

    private Timer currentTimer;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
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
    
    void Update()
    {
        amount = timersList.Count;
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        for (int i = timersList.Count - 1; i >= 0; i--)
        {
            currentTimer = timersList[i];
            UpdateTimer(currentTimer);
        }
    }

    private void UpdateTimer(Timer timer)
    {
        if(timer.ToDelete())
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
                {
                    timer.Restart();
                }
                else
                {
                    timer.ResetTimer();   
                }
            }
            timer.OnValueChanged().Invoke();
        }
    }

    public static TimersController GetController()
    {
        return instance;
    }
    
}
