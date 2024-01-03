using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderTimer : MonoBehaviour
{
    //[SerializeField]
    //private float duration;

    private Slider slider;
    private Timer timer;
    
    void Start()
    {
        //timer = gameObject.AddComponent<Timer>();
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        //timer.SetTimer(duration);
        //slider.minValue = 0f;
        //slider.maxValue = timer.GetValue();
        //slider.value = timer.GetValue();
    }

    public void SetTimerSlider(float duration)
    {
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(duration);
        slider.maxValue = timer.GetValue();
        //slider.value = timer.GetValue();
        slider.value = 0f;
    }

    public void SetTimerSlider(Timer timer)
    {
        this.timer = timer;
        slider.maxValue = timer.GetValue();
        //slider.value = timer.GetValue();
        slider.value = 0f;
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
    
    public void ResetTimer()
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
