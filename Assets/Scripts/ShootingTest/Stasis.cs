using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Timer))]
public class Stasis : MonoBehaviour
{
    [SerializeField]
    private float slowRatio = 2.0f;
    [SerializeField]
    private Color color; //for test

    [SerializeField]
    private float duration = 1.0f;

    private GameObject target;
    [SerializeField]
    private Timer timer;

    private static ArrayList affectedObjects = new ArrayList();
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
            if(!affectedObjects.Contains(target))
            {
                gameObject.SetActive(false);
                affectedObjects.Add(target);
                timer.Activate();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTimerStart()
    {
        SlowDownTimers();
        SlowDownObject();
    }

    private void OnTimerEnd()
    {
        affectedObjects.Remove(target);
        SpeedUpTimers();
        SpeedUpObject();
        timer.Remove();
        Destroy(gameObject);
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
            if(target.GetComponent<SpriteRenderer>() != null) //for test
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
