using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float _value;
    private float _lastValue;
    private bool _isActive;
    private bool _isContinuous;
    
    public Timer(float startValue, bool isContinuous)
    {
        _value = startValue;
        _lastValue = startValue;
        _isActive = false;
        _isContinuous = isContinuous;
        TimersController.FindController().AddTimer(this);
    }
    public float GetValue()
    {
        return _value;
    }

    public void SetValue(float newValue)
    {
        _value = newValue;
    }

    public bool IsActive()
    {
        return _isActive;
    }
    
    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }

    public bool IsContinuous()
    {
        return _isContinuous;
    }

    public void SetContinuous(bool isContinuous)
    {
        _isContinuous = isContinuous;
    }

    public void ResetValue(float newValue)
    {
        _lastValue = newValue;
    }
    
    public void Reset()
    {
        _value = _lastValue;
        _isActive = false;
    }

    public void Restart()
    {
        _value = _lastValue;
        _isActive = true;
    }

    public void Stop()
    {
        _isActive = false;
    }

    public void Continue()
    {
        _isActive = true;
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Remove()
    {
        TimersController.FindController().RemoveTimer(this);
    }
}
