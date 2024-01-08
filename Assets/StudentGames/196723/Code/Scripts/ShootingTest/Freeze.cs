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

    [SerializeField]
    private GameObject snowParticles;

    ParticleSystem createdSystem;
    private GameObject target;
    private Timer timer;

    private int firstHit = 0;

    private static ArrayList affectedObjects = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        timer = gameObject.AddComponent<Timer>();
        timer.SetTimer(duration);
        timer.OnStart().AddListener(OnTimerStart);
        timer.OnEnd().AddListener(OnTimerEnd);
        GetComponent<ParticleSystem>().Emit(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.isTrigger)
        {
            if(gameObject.CompareTag("World"))
            {
                Destroy(gameObject);
            }

            if(affectsPlayer)
            {
                if(!collision.CompareTag("Enemy") || (collision.CompareTag("Enemy") && firstHit != 0))
                {
                    AddFreezeEffect(collision);
                }
                else
                {
                    firstHit++;
                }
            }
            else
            {
                if(!collision.CompareTag("Player"))
                {
                    AddFreezeEffect(collision);
                }
            }
        }
    }

    private void AddFreezeEffect(Collider2D collision)
    {
        target = collision.gameObject;
        if (!affectedObjects.Contains(target))
        {
            GameObject snowEffect = Instantiate(snowParticles, target.transform.position, Quaternion.identity);
            createdSystem = snowEffect.GetComponent<ParticleSystem>();
            createdSystem.Stop();
            ParticleSystem.ShapeModule shapeModule = createdSystem.shape;
            ParticleSystem.MainModule mainModule = createdSystem.main;
            mainModule.duration = duration + mainModule.startLifetime.constant;
            snowEffect.transform.parent = target.transform;
            shapeModule.scale = Vector3.Scale(target.GetComponent<Renderer>().bounds.size, target.transform.localScale);
            createdSystem.Play();

            gameObject.SetActive(false);
            affectedObjects.Add(target);
            timer.Activate();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTimerStart()
    {
        SlowDownTimers();
        SlowDownObject();
    }

    private void OnTimerEnd()
    {
        if(gameObject != null)
        {
            affectedObjects.Remove(target);
            SpeedUpTimers();
            SpeedUpObject();
            timer.Remove();
            if(createdSystem != null)
            {
                createdSystem.Stop();
            }
            Destroy(gameObject);
        }
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
            Moving moving = target.GetComponent<Moving>();
            if (moving != null)
            {
                moving.SlowDown(slowRatio);
            }
            if (target.GetComponent<SpriteRenderer>() != null)
            {
                target.GetComponent<SpriteRenderer>().color = color;
            }

            if (target.GetComponent<Animator>() != null)
            {
                target.GetComponent<Animator>().speed /= slowRatio;
            }
        }
    }

    public void SpeedUpObject()
    {
        if(target != null)
        {
            if(target.GetComponent<SpriteRenderer>() != null)
            {
                target.GetComponent<SpriteRenderer>().color = Color.white;
            }
            Moving moving = target.GetComponent<Moving>();
            if (moving != null)
            {
                moving.SpeedUp(slowRatio);
            }
            if (target.GetComponent<Animator>() != null)
            {
                target.GetComponent<Animator>().speed *= slowRatio;
            }
        }
    }

}
