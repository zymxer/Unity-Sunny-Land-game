using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TimersController : MonoBehaviour
{
    private readonly ArrayList _timersList = new ArrayList();

    public ArrayList GetTimersList()
    {
        return _timersList;
    }

    public void AddTimer(Timer timer)
    {
        _timersList.Add(timer);
    }

    public void RemoveTimer(Timer timer) 
    {
        _timersList.Remove(timer);
    }
    
    void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        foreach (Timer timer in _timersList)
        {
            UpdateTimer(timer);
        }
    }

    private void UpdateTimer(Timer timer)
    {
        if (timer.IsActive())
        {
            timer.SetValue(timer.GetValue() - Time.deltaTime);
            if (timer.GetValue() <= 0f)
            {
                if (timer.IsContinuous())
                {
                    timer.Restart();
                }
                else
                {
                    timer.Reset();   
                }
            }
        }
    }

    public static TimersController FindController()
    {
        return GameObject.Find("TimersController").GetComponent<TimersController>();
    }
    
}
