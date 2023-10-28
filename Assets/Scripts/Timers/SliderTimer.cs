using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class SliderTimer : MonoBehaviour
{

    private Slider _slider;
    private Timer _timer;
    
    void Start()
    {
        _timer = GetComponent<Timer>();
        _slider = GetComponent<Slider>();
        _slider.minValue = 0f;
        _slider.maxValue = _timer.GetValue();
        _slider.value = _timer.GetValue();
    }
    
    void Update()
    {
        if (_timer.IsActive())
        {
            _slider.value = _timer.GetValue();
        }
    }
    
    public Slider GetSlider()
    {
        return _slider;
    }

    public Timer GetTimer()
    {
        return _timer;
    }

    public void SetContinuous(bool newContinuous)
    {
        _timer.SetContinuous(newContinuous);
    }

    public void ResetContinuous()
    {
        _timer.SetContinuous(!_timer.IsContinuous());
    }
    
    public void Reset()
    {
        _timer.ResetTimer();
        _slider.value = _timer.GetValue();
    }

    public void Restart()
    {
        _timer.Restart();
        _slider.value = _timer.GetValue();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Continue()
    {
        _timer.Continue();
    }

    public void Activate()
    {
        _timer.Activate();
    }
}
