using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class Stasis : MonoBehaviour
{
    [SerializeField]
    private float slowRatio = 2.0f;
    [SerializeField]
    private Color color;

    [SerializeField]
    private float duration = 1.0f;

    private GameObject target;
    [SerializeField]
    private Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        timer.OnStart().AddListener(OnTimerStart);
        timer.OnEnd().AddListener(OnTimerEnd);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("PlatformTop"))
        {
            target = collision.gameObject;
            //timer = collision.gameObject.AddComponent<Timer>();
            //timer.ResetValue(duration);
            timer.Activate();
        }
    }

    private void OnTimerStart()
    {
        SlowDownTimers();
        SlowDownObject();
    }

    private void OnTimerEnd()
    {
        SpeedUpTimers();
        SpeedUpObject();
        timer.Remove();
    }

    private void SlowDownTimers()
    {
        if (target != null)
        {
            Timer[] timers = target.GetComponents<Timer>();
            foreach (Timer timer in timers)
            {
                timer.SlowDown(slowRatio);
            }
        }
    }

    private void SpeedUpTimers()
    {
        if(target != null)
        {
            Timer[] timers = target.GetComponents<Timer>();
            foreach (Timer timer in timers)
            {
                timer.SpeedUp(slowRatio);
            }
        }
    }

    private void SlowDownObject()
    {
        if(target != null)
        {
            target.GetComponent<SpriteRenderer>().color = color;
            MovingPlatform movingPlatform = target.GetComponent<MovingPlatform>();
            if (movingPlatform != null)
            {
                movingPlatform.SlowDown(slowRatio);
            }
        }
        // more and more
    }

    public void SpeedUpObject()
    {
        if(target != null)
        {
            target.GetComponent<SpriteRenderer>().color = Color.white;
            MovingPlatform movingPlatform = target.GetComponent<MovingPlatform>();
            if (movingPlatform != null)
            {
                movingPlatform.SpeedUp(slowRatio);
            }
        }
    }

}
