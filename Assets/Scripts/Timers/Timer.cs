using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    private float value;
    private bool isActive;
    private bool isContinuous;
    private UnityEvent onStart = new UnityEvent();
    private UnityEvent onValueChanged = new UnityEvent();
    private UnityEvent onEnd = new UnityEvent();

    private float startValue;
    private float prevoiosValue;
    private float delta;
    private bool toDelete = false;

    private void Start()
    {
        startValue = value;
        prevoiosValue = value;
        TimersController.GetController().AddTimer(this);
    }

    //active = false, continuous = false
    public void SetTimer(float timerValue)
    {
        value = timerValue;
        isActive = false;
        isContinuous = false;
    }
    public void SetTimer(float timerValue, bool timerActive, bool timerContinuous)
    {
        value = timerValue;
        isActive = timerActive;
        isContinuous = timerContinuous;
    }

    public float GetValue()
    {
        return value;
    }

    public float TimePast()
    {
        return Mathf.Max(startValue - value, 0.0f);
    }

    public float TimePastPercent()
    {
        return (startValue - value) / startValue;
    }

    public void SlowDown(float ratio)
    {
        startValue *= ratio;
        value *= ratio;
    }

    public void SpeedUp(float ratio)
    {
        startValue /= ratio;
        value /= ratio;
    }

    public float GetDelta()
    {
        return delta;
    }

    public void UpdateDelta()
    {
        delta = prevoiosValue - value;
        delta = Mathf.Max(delta, 0.0f);
        prevoiosValue = value;
    }

    public UnityEvent OnStart()
    {
        return onStart;
    }
    public UnityEvent OnValueChanged()
    {
        return onValueChanged;
    }
    public UnityEvent OnEnd()
    {
        return onEnd;
    }

    public void SetValue(float newValue)
    {
        value = newValue;
    }

    public bool IsActive()
    {
        return isActive;
    }
    
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public bool IsContinuous()
    {
        return isContinuous;
    }

    public void SetContinuous(bool isContinuous)
    {
        this.isContinuous = isContinuous;
    }

    public void ResetValue(float newValue)
    {
        startValue = newValue;
        value = newValue;
        prevoiosValue = newValue;
    }
    
    public void ResetTimer()
    {
        value = startValue;
        prevoiosValue = startValue;
        isActive = false;
    }

    public void Restart()
    {
        value = startValue;
        prevoiosValue = startValue;
        isActive = true;
    }

    public void RestartWithOnStart()
    {
        value = startValue;
        prevoiosValue = startValue;
        isActive = true;
        onStart.Invoke();
    }

    public void Stop()
    {
        isActive = false;
    }

    public void End()
    {
        value = -1f;
    }

    public void Continue() //Doesn't invoke OnStart
    {
        isActive = true;
    }

    public void Activate() //Invokes OnStart
    {
        isActive = true;
        onStart.Invoke();
    }


    public void Remove()
    {
        toDelete = true;
    }

    public bool ToDelete()
    {
        return toDelete;
    }

}
