using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    private float _value;
    private float _lastValue;
    private bool _isActive;
    private bool _isContinuous;
    private UnityEvent _onStart = new UnityEvent();
    private UnityEvent _onValueChanged = new UnityEvent();
    private UnityEvent _onEnd = new UnityEvent();

    public Timer(float startValue, bool isContinuous)
    {
        _value = startValue;
        _lastValue = startValue;
        _isActive = false;
        _isContinuous = isContinuous;
        TimersController.GetController().AddTimer(this);
    }
    public float GetValue()
    {
        return _value;
    }

    public UnityEvent OnStart()
    {
        return _onStart;
    }
    public UnityEvent OnValueChanged()
    {
        return _onValueChanged;
    }
    public UnityEvent OnEnd()
    {
        return _onEnd;
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

    public void Continue() //Doesn't invoke OnStart
    {
        _isActive = true;
    }

    public void Activate() //Invokes OnStart
    {
        _isActive = true;
        _onStart.Invoke();
    }

    public void Remove()
    {
        _onStart.RemoveAllListeners();
        _onValueChanged.RemoveAllListeners();
        _onEnd.RemoveAllListeners();
        TimersController.GetController().RemoveTimer(this);
    }

}
