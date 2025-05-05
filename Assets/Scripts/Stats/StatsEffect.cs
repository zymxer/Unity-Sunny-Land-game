using System.Collections.Generic;
using UnityEngine;

public class StatsEffect : MonoBehaviour
{
    private static readonly List<StatsEffect> playerEffects = new();
    private StatsContainer container;
    private float duration;
    private float speed;

    private Timer timer;
    private StatType type;

    public static StatsEffect AddEffect(GameObject target, StatType type, float speed, float duration)
    {
        if (target.GetComponent<StatsContainer>() != null)
        {
            var effectObject = new GameObject();
            effectObject.name = "Stat Effect";
            effectObject.transform.parent = target.transform;
            effectObject.AddComponent<StatsEffect>();
            effectObject.AddComponent<Timer>();
            effectObject.GetComponent<StatsEffect>().StartEffect(target, type, speed, duration);
            if (target.CompareTag("Player")) playerEffects.Add(effectObject.GetComponent<StatsEffect>());
            return effectObject.GetComponent<StatsEffect>();
        }

        return null;
    }

    public void StartEffect(GameObject target, StatType type, float speed, float duration)
    {
        this.type = type;
        this.speed = speed;
        this.duration = duration;
        container = target.GetComponent<StatsContainer>();
        timer = GetComponent<Timer>();
        timer.SetTimer(duration);
        timer.OnValueChanged().AddListener(OnTimerChange);
        timer.OnEnd().AddListener(OnTimerEnd);
        timer.Activate();
    }

    public void StopEffect()
    {
        OnTimerEnd();
    }

    private void OnTimerChange()
    {
        if (type == StatType.MANA)
        {
            container.ChangeMana(speed * timer.GetDelta());
        }
        else if (type == StatType.HEALTH)
        {
            if (container.Health <= 0.0f) StopEffect();
            container.ChangeHP(speed * timer.GetDelta());
        }
    }

    private void OnTimerEnd()
    {
        timer.Remove();
        Destroy(gameObject);
    }

    public static void ClearPlayerEffects()
    {
        for (var i = playerEffects.Count - 1; i >= 0; i--)
        {
            if (playerEffects[i] != null) playerEffects[i].StopEffect();
            playerEffects.RemoveAt(i);
        }
    }
}