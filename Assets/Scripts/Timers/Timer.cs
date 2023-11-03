using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float value;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private bool isContinuous;
    [Space]
    [SerializeField]
    private UnityEvent onStart = new UnityEvent();
    [SerializeField]
    private UnityEvent onValueChanged = new UnityEvent();
    [SerializeField]
    private UnityEvent onEnd = new UnityEvent();

    private float startValue;
    private float prevoiosValue;
    private float delta;
    private bool toDelete = false;

    private void Start()
    {
        startValue = value;
        TimersController.GetController().AddTimer(this);
    }

    public float GetValue()
    {
        return value;
    }

    public float TimePast()
    {
        return startValue - value;
    }

    public float TimePastPercent()
    {
        return (startValue - value) / startValue;
    }

    public float GetDelta()
    {
        return delta;
    }

    public void UpdateDelta()
    {
        delta = prevoiosValue - value;
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
    }
    
    public void ResetTimer()
    {
        value = startValue;
        isActive = false;
    }

    public void Restart()
    {
        value = startValue;
        isActive = true;
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
