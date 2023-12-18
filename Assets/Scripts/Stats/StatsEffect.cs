using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class StatsEffect : MonoBehaviour
{
    private StatType type;
    private float speed;
    private float duration;
    private StatsContainer container;

    private Timer timer;

    public static void AddEffect(GameObject target, StatType type, float speed, float duration)
    {
        if (target.GetComponent<StatsContainer>() != null)
        {
            GameObject effectObject = new GameObject();
            effectObject.name = "Stat Effect";
            effectObject.transform.parent = target.transform;
            effectObject.AddComponent<StatsEffect>();
            effectObject.AddComponent<Timer>();
            effectObject.GetComponent<StatsEffect>().StartEffect(target, type, speed, duration);
        }
    }

    public void StartEffect(GameObject target, StatType type, float speed, float duration)
    {
        this.type = type;
        this.speed = speed;
        this.duration = duration;
        container = target.GetComponent<StatsContainer>();
        timer = GetComponent<Timer>();
        timer.ResetValue(duration);
        timer.OnValueChanged().AddListener(OnTimerChange);
        timer.OnEnd().AddListener(OnTimerEnd);
        timer.Activate();
    }

    private void OnTimerChange()
    {
        if (type == StatType.MANA)
        {
            if (speed >= 0.0f)
            {
                container.IncreaseMana(speed * timer.GetDelta());
            }
            else
            {
                container.DecreaseMana(-speed * timer.GetDelta());
            }

        }
        else if (type == StatType.HEALTH)
        {
            if (speed >= 0.0f)
            {
                container.IncreaseHP(speed * timer.GetDelta());
            }
            else
            {
                container.DecreaseHP(-speed * timer.GetDelta());
            }
        }
    }

    private void OnTimerEnd()
    {
        timer.Remove();
        Destroy(gameObject);
    }
}
