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

    private ParticleSystem createdSystem;
    private GameObject target;
    private Timer timer;

    private int firstHit = 0;
    private Color ogColor;

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
            if(collision.CompareTag("World"))
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
            //shapeModule.scale = Vector3.Scale(target.GetComponent<Renderer>().bounds.size, target.transform.localScale);
            //shapeModule.scale.Set(target.GetComponent<Renderer>().bounds.size.x * target.transform, 1.0f, target.GetComponent<Renderer>().bounds.size.y);
            Vector3 newScale = new Vector3(target.GetComponent<Collider2D>().bounds.size.x, 1.0f, target.GetComponent<Collider2D>().bounds.size.y);
            shapeModule.scale = newScale;
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
        SpeedUpObject();
        SpeedUpTimers();
        if (createdSystem != null)
        {
            createdSystem.Stop();
        }
        affectedObjects.Remove(target);
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
            Moving moving = target.GetComponent<Moving>();
            if (moving != null)
            {
                moving.SlowDown(slowRatio);
            }
            if (target.GetComponent<SpriteRenderer>() != null)
            {
                ogColor = target.GetComponent<SpriteRenderer>().color;
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
                target.GetComponent<SpriteRenderer>().color = ogColor;
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
