using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [SerializeField]
    private float slowRatio = 2.0f;
    [SerializeField]
    private Color color; //for test

    [SerializeField]
    private float duration = 1.0f;

    [SerializeField]
    private bool affectsPlayer;

    private GameObject target;
    private Timer timer;

    private static ArrayList affectedObjects = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(duration);
        timer.OnStart().AddListener(OnTimerStart);
        timer.OnEnd().AddListener(OnTimerEnd);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!collision.gameObject.CompareTag("Player") || (collision.gameObject.CompareTag("Player") && affectsPlayer)) 
            && !collision.gameObject.CompareTag("World"))
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
            Moving moving = target.GetComponent<Moving>();
            if (moving != null)
            {
                moving.SlowDown(slowRatio);
            }
        }
        // more and more
    }

    public void SpeedUpObject()
    {
        if(target != null)
        {
            target.GetComponent<SpriteRenderer>().color = Color.white;
            Moving moving = target.GetComponent<Moving>();
            if (moving != null)
            {
                moving.SpeedUp(slowRatio);
            }
        }
    }

}
