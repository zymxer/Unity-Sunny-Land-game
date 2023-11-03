using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Timer))]
public class SliderTimer : MonoBehaviour
{

    private Slider slider;
    private Timer timer;
    
    void Start()
    {
        timer = GetComponent<Timer>();
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = timer.GetValue();
        slider.value = timer.GetValue();
    }
    
    void Update()
    {
        if (timer.IsActive())
        {
            slider.value = timer.GetValue();
        }
    }
    
    public Slider GetSlider()
    {
        return slider;
    }

    public Timer GetTimer()
    {
        return timer;
    }

    public void SetContinuous(bool newContinuous)
    {
        timer.SetContinuous(newContinuous);
    }

    public void ResetContinuous()
    {
        timer.SetContinuous(!timer.IsContinuous());
    }
    
    public void Reset()
    {
        timer.ResetTimer();
        slider.value = timer.GetValue();
    }

    public void Restart()
    {
        timer.Restart();
        slider.value = timer.GetValue();
    }

    public void Stop()
    {
        timer.Stop();
    }

    public void Continue()
    {
        timer.Continue();
    }

    public void Activate()
    {
        timer.Activate();
    }
}
