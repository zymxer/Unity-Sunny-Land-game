using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float _value;
    [SerializeField]
    private bool _isActive;
    [SerializeField]
    private bool _isContinuous;
    [Space]
    [SerializeField]
    private UnityEvent _onStart = new UnityEvent();
    [SerializeField]
    private UnityEvent _onValueChanged = new UnityEvent();
    [SerializeField]
    private UnityEvent _onEnd = new UnityEvent();

    private float _startValue;
    private float _prevoiosValue;
    private float _delta;

    private void Start()
    {
        _startValue = _value;
        TimersController.GetController().AddTimer(this);
    }

    public float GetValue()
    {
        return _value;
    }

    public float TimePast()
    {
        return _startValue - _value;
    }

    public float TimePastPercent()
    {
        return (_startValue - _value) / _startValue;
    }

    public float GetDelta()
    {
        return _delta;
    }

    public void UpdateDelta()
    {
        _delta = _prevoiosValue - _value;
        _prevoiosValue = _value;
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
        _startValue = newValue;
    }
    
    public void ResetTimer()
    {
        _value = _startValue;
        _isActive = false;
    }

    public void Restart()
    {
        _value = _startValue;
        _isActive = true;
    }

    public void Stop()
    {
        _isActive = false;
    }

    public void End()
    {
        _value = -1f;
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
