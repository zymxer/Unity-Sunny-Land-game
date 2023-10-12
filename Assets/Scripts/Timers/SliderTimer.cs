using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    [SerializeField] private float value;
    [SerializeField] private bool isContinuous;
    private Slider _slider;
    private Timer _timer;
    
    void Start()
    {
        _slider = gameObject.GetComponent<Slider>();
        _slider.minValue = 0f;
        _slider.maxValue = value;
        _slider.value = value;

        _timer = new Timer(value, isContinuous);
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
        _timer.Reset();
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
